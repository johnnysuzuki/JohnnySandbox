using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MetroidvaniaTools
{
    public class AimManager : Abilities
    {
        protected GameObject whereToAimPoint;
        public Transform whereToAim;
        public Transform origin;
        [HideInInspector]
        public Vector2 mousePosition;
        public Vector2 fromOriginToAim;


        protected override void Initialization()
        {
            base.Initialization();
            StartCoroutine("Positioning");
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            AimPositionSetting();
            if (!grapplingHook.grappling)
            {
                CheckDirection();
            }
        }
        protected virtual void FixedUpdate()
        {
        }

        protected virtual void AimPositionSetting()
        {
            Vector2 mouse = Input.mousePosition;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouse);//現在マウスの位置を取得
            if (!grapplingHook.grappling)
            {
                whereToAim.position = mousePosition;//狙う場所はマウスの座標と同じ場所
            }
            if (weapon.currentProjectile != null &&  weapon.currentProjectile.activeSelf && weapon.currentProjectile.tag == "GrapplingHook")
             //グラップルフックが発射された時
            {
                whereToAim.position = weapon.currentProjectile.transform.position;
            }
            fromOriginToAim = whereToAim.position - origin.position;//狙う場所と原点の差分ベクトル
 　　　　　 if (character.isFacingLeft)
            {
                origin.rotation = Quaternion.FromToRotation(Vector2.left, fromOriginToAim);//差分ベクトルから角度を出す。
            }
            else
            {
                origin.rotation = Quaternion.FromToRotation(Vector2.right, fromOriginToAim);//差分ベクトルから角度を出す
            }
        }

        IEnumerator Positioning()
        {
            whereToAimPoint = GameObject.FindGameObjectWithTag("WhereToAim");
            if (whereToAimPoint == null)
            {
                yield return null;
            }
            else
            {
                whereToAim = whereToAimPoint.transform;
            }
        }

        //マウスの位置によってキャラクターの向きを変更する
        protected virtual void CheckDirection()
        {
            if (fromOriginToAim.x > 0)
            {
                if (character.isFacingLeft)
                {
                    character.isFacingLeft = false;
                    Flip();
                }
            }
            if (fromOriginToAim.x < 0)
            {
                if (!character.isFacingLeft)
                {
                    character.isFacingLeft = true;
                    Flip();
                }
            }
        }
    }
}