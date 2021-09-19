using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MetroidvaniaTools
{ 
    public class Character : MonoBehaviour
    {
        [HideInInspector]
        public bool isFacingLeft;
        [HideInInspector]
        public bool isGrounded;
        [HideInInspector]
        public bool isDashing;
        [HideInInspector]
        public bool isWallSliding;

        public static float gravity = -9.81f;

        protected Collider2D col;
        protected Rigidbody2D rb;
        protected Horizontalmovement movement;
        protected Jump jump;
        protected ObjectPooler objectPooler;
        protected AimManager aimManager;
        protected Weapon weapon;

        private Vector2 facingLeft;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();

        }
        private void Update()
        {
        }

        protected virtual void Initialization()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            movement = GetComponent<Horizontalmovement>();
            jump = GetComponent<Jump>(); 
            objectPooler = ObjectPooler.Instance;
            aimManager = GetComponent<AimManager>();
            weapon = GetComponent<Weapon>();
            facingLeft = new Vector2(-transform.localScale.x,transform.localScale.y);
        }

        protected virtual void Flip()
        {
            if (isFacingLeft)
            {
                transform.localScale = facingLeft;
            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }

        }

        protected virtual bool CollisionCheck(Vector2 direction , float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool SideCheck(float distance,LayerMask collision)//向いてる方向の当たり判定を探す
        {
            if (!isFacingLeft && CollisionCheck(Vector2.right, distance, collision) || isFacingLeft && CollisionCheck(Vector2.left,distance,collision))
            {
                return true;
            }
            else
                return false;

        }


        protected virtual bool Falling(float velocity)
        {
            if (!isGrounded && rb.velocity.y < velocity)
            {
                return true;
            }
            else
                return false;
        }

        protected virtual void FallSpeed(float velocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, velocity);
        }


    }

}

