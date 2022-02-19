using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MetroidvaniaTools
{
    public class ChronoBreak : Abilities
    {
        /*このプログラムにおいて想定している処理は、
         * ①A:ボタンを押して現在位置を記録（記録位置と呼称）
         * ①B:ワープ可能最大時間までの時間計測を開始する
         * ┣②再度ボタンを押すと記録位置にワープする
         * ┗③ワープ可能最大時間になるとワープ使用不可となる
         * 　┗④アビリティそのもののクールダウンを開始、計算する
         */

        /*必要となる変数のメモ
         * ①ワープ可能最大時間
         * ②アビリティクールダウン
         * ③戻ってくる場所
         */

        [SerializeField]
        protected float warpTime; //ワープ可能最大時間
        protected float warpTimeCountDown; // ワープ可能かどうかを数える
        [SerializeField]
        protected float warpCoolDown; //アビリティクールダウン
        protected float warpCoolDownCountDown; //アビリティクールダウンの計測
        [SerializeField]
        protected Vector2 backPosition; //戻ってくる場所
        protected bool canWarp; //今ワープができるかどうか
        protected bool existWarpPosition; //戻ってくる場所が存在するかどうか
        protected bool canGoWarp; //ワープに帰ってるかどうか
        [SerializeField]
        GameObject originCharacter; //コピー元のオブジェクト
        

        protected override void Initialization()
        {
            base.Initialization();
            canWarp = true;
            existWarpPosition = false;
            canGoWarp = true;
        }

        protected virtual void Update()
        {
            WarpPressed();
        }

        protected virtual void WarpPressed()
        {
            if (Input.GetButtonDown("Warp"))
            {
                if (existWarpPosition == true)
                {
                    Warping(); //戻ってくる場所があるならワープする
                }
                if (canWarp == true)
                {
                    WarpSetting(); //ワープ可能なら戻ってくる場所を設置する
                }
            }
        }


        protected virtual void WarpSetting()
        {
            backPosition = transform.position;
            canWarp = false;
            existWarpPosition = true;
            StartCoroutine("WarpCounting");
        }

        protected virtual void Warping()　//戻ってくる場所があるならワープする
        {
            transform.position = backPosition;
            existWarpPosition = false;
            canGoWarp = false;
            StartCoroutine("WarpCoolDown");
        }


        IEnumerator WarpCounting() //ワープ可能時間を計測するコルーチン
        {
            /*
             * ①クールダウンの宣言、
             * while(クールダウンが０以上のとき)
             * {
             * クールダウン変数－Time.deltatime
             * 
             * }
             * 
             */
            warpTimeCountDown = warpTime; //ワープ可能時間計測用変数
            while(warpTimeCountDown >= 0) //ワープ可能限界時間に至るまでループ
            {
                warpTimeCountDown -= Time.deltaTime;
                if(canGoWarp == false) //時間内に動けば早めに消える
                {
                    existWarpPosition = false;
                    break;
                }
                yield return null;
            }
            existWarpPosition = false; //ワープ可能時間が経てば設置判定が消える
        }

        IEnumerator WarpCoolDown() //ワープのクールダウンを計測するコルーチン
        {
            warpCoolDownCountDown = warpCoolDown; //クールダウンの宣言
            while(warpCoolDownCountDown >= 0)
            {
                warpCoolDownCountDown -= Time.deltaTime;
                yield return null;
            }
            canWarp = true; //クールダウンが終われば再び使えるようになる
            canGoWarp = true;
        }

    }
}