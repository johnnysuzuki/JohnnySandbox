using UnityEngine;
using System.Collections;

public class playermanager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;
    public float dashwait;
    public bool OnGround { get; set; }

    //　方向の列挙
    public enum DIRECTION_TYPE 
    {
        STOP,
        RIGHT,
        LEFT,
    }

    IEnumerator Dash()
    {
        if (!isDead)
        {
            if(dwait == true)
            {
                dflug = true;
                float dspeed;
                if (transform.localScale.x == 1)
                {
                    dspeed = 3f;
                }
                else
                {
                    dspeed = -3f;
                }
                dspeed = dspeed * 10f;
                int dashsecond = 0;
                while (dashsecond < 20)
                {
                    Debug.Log(dashsecond);
                    dashsecond++;
                    rigidbody2d.velocity = new Vector2(dspeed, 0);
                    yield return null;
                }
                dflug = false;
                dwait = false;
                StartCoroutine(Dashwait());


            }


        }
    }

    IEnumerator Dashwait()
    {
        yield return new WaitForSeconds(dashwait);
        dwait = true;

    }

    //初期方向
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    //rigidbody2dを読み込むための関数を作成
    Rigidbody2D rigidbody2d;
    //speedを表す関数を作成
    float speed;
    //アニメーターコンポーネント
    Animator animator;
    //ジャンプ可能回数
    float jumpn;
    
    float jumppower = 400;
    //生死判定
    bool isDead = false;

    //ダブルジャンプ可能かどうか
    bool canjump = false;
    //ダッシュ可能かどうか
    bool dflug = false;
    //ダッシュを待つための関数
    bool dwait = true;
    

    //rigidbody2dを取得する
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpn = 0;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        //左右入力に応じて-1,0,1のどれかを取る関数xを作る
        float x = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed",Mathf.Abs(x));

        if (x == 0) 
        {
            //　止まっている
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0) 
        {
            //　右へ
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            //　左へ
            direction = DIRECTION_TYPE.LEFT;
        }


        //ダブルジャンプ不可能である場合
        if (canjump == false)
        {
            if (IsGround())
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
                else
                {
                    animator.SetBool("Isjumping", false);
                }
            }

        }
        else
        {

            //ジャンプ回数のリセットと追加
            if (IsGround() && jumpn < 1)
            {
                jumpn = 1;
            }

            // スペースが押されたらJUMPさせる
            if (jumpn >= 1 && Input.GetButtonDown("Jump"))
            {
                jumpn = jumpn - 1;
                Jump();
            }

            //接地時アニメーションをキャンセルする
            if (IsGround())
            {
                animator.SetBool("Isjumping", false);
            }

        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Dash");
            StartCoroutine(Dash());

        }

    }
   

    private void FixedUpdate()
    {

        if (isDead)
        {
            return;
        }

        //入力された方向に従って速度を加減する
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale = new Vector3(-1, 1, 1);
                break;

        }

        //ダッシュが0でなければ
        if (dflug == false)
        {
            //走る際に速度が倍になる。走るキーはshiftとする
            if (Input.GetKey("left shift"))
            {
                speed = speed * 2;
                rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
            }
            else
            {
                rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);
            }

        }



    }

    void Jump()
    {
        //一旦上下の速度を0にする
        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x,0);
        //上に力を加える
        rigidbody2d.AddForce(Vector2.up * jumppower);
        animator.SetBool("Isjumping",true);
    }
    
    //地面と接触しているかどうかを判定する関数
    bool IsGround()
    {
        // 始点と終点を作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 RightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer)
            || Physics2D.Linecast(RightStartPoint, endPoint, blockLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "trap") 
        {
            Debug.Log("game over");
            PlayerDeath();

        }

        if(collision.gameObject.tag == "Finish")
        {
            Debug.Log("clear");
            gameManager.GameClear();
        }


        if (collision.gameObject.tag == "Item")
        {
            //アイテム取得
            //ぶつかったアイテムのアイテムマネージャーからゲットアイテムを起動させる
            collision.gameObject.GetComponent<itemManager>().GetItem();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyManager enemy = collision.gameObject.GetComponent<EnemyManager>();
            if (this.transform.position.y + 0.3f > enemy.transform.position.y)
            {
                //上から踏んだら、敵を削除
                enemy.DestroyEnemy();
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
                Jump();
            }
            else
            {
                //横からぶつかったら
                PlayerDeath();

            }

        }
        if(collision.gameObject.tag == "Gem")
        {
            collision.gameObject.GetComponent<itemManager>().GetGem();
            canjump = true;
        }


    }

    void PlayerDeath()
    {
        isDead = true;
        rigidbody2d.velocity = new Vector2(0, 0);
        rigidbody2d.AddForce(Vector2.up * jumppower);
        animator.Play("Player-death");
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(boxCollider2D);
        gameManager.GameOver();

    }


}
