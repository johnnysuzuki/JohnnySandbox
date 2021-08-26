using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameObject deathEffect;
    //　方向の列挙
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    //初期方向
    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    //rigidbody2dを読み込むための関数を作成
    Rigidbody2D rigidbody2d;
    //speedを表す関数を作成
    float speed;

    //rigidbody2dを取得する
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //右へ
        direction = DIRECTION_TYPE.RIGHT;
    }

    private void Update()
    {
            if (!IsGround())
            {
                //方向を変える
                ChangeDirection();

            }
    }

    private void FixedUpdate()
    {
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
        rigidbody2d.velocity = new Vector2(speed, rigidbody2d.velocity.y);

    }

    bool IsGround()
    {
        Vector3 startVec = transform.position + transform.right * 0.5f *transform.localScale.x;
        Vector3 endVec = startVec - transform.up * 0.5f;
        Debug.DrawLine(startVec, endVec);
        return Physics2D.Linecast(startVec,endVec,blockLayer);
    }

    void ChangeDirection()
    {
        if(direction == DIRECTION_TYPE.RIGHT)
        {
            direction = DIRECTION_TYPE.LEFT;
        }
        else if (direction == DIRECTION_TYPE.LEFT)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
        Instantiate(deathEffect, this.transform.position, this.transform.rotation);
    }

}
