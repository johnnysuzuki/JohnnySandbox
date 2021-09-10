using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Jump : Abilities
    {
        [SerializeField]
        protected bool LimitAirJumps;
        [SerializeField]
        protected int MaxJumpNum;
        [SerializeField]
        protected float jumpForce;//ジャンプ力
        [SerializeField]
        protected float HoldForce;
        [SerializeField]
        protected float ButtonHoldTime;
        [SerializeField]
        protected float distanceToCollider;//あたりはんてい
        [SerializeField]
        protected float MaxJumpSpeed;
        [SerializeField]
        protected float MaxFallSpeed;
        [SerializeField]
        protected float AcceptedFallSpeed;

        public LayerMask collisionLayer;//何処とぶつかるか

        private bool isJumping;//ジャンプ中か否か
        private int NumberOfJumpsLeft;
        private float JumpCountDown;

        protected override void Initialization()
        {
            base.Initialization();
            NumberOfJumpsLeft = MaxJumpNum;
            JumpCountDown = ButtonHoldTime;
        }

        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();
        }
        protected virtual bool JumpPressed()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (!character.isGrounded && NumberOfJumpsLeft == MaxJumpNum)
                {
                    isJumping = false;
                    return false;
                }
                if(LimitAirJumps && Falling(AcceptedFallSpeed))
                {
                    isJumping = false;
                    return false;
                }
                NumberOfJumpsLeft--;
                if (NumberOfJumpsLeft >= 0)
                {
                    JumpCountDown = ButtonHoldTime;
                    isJumping = true;
                }
                return true;
            }
            else
                return false;
        }

        protected virtual bool JumpHeld()
        {
            if (Input.GetButton("Jump"))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
          
            if (isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up*jumpForce) ;
                AdditionalAir();
            }
            if (rb.velocity.y > MaxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, MaxJumpSpeed);
            }
        }

        protected virtual void AdditionalAir()
        {
            if (JumpHeld())
            {
                JumpCountDown -= Time.deltaTime;
                if (JumpCountDown <= 0)
                {
                    JumpCountDown = 0;
                    isJumping = false;
                }
                else
                    rb.AddForce(Vector2.up * HoldForce);
            }
            else
                isJumping = false;
        }

        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer)&& !isJumping)
            {
                character.isGrounded = true;
                NumberOfJumpsLeft = MaxJumpNum;
            }
            else
            {
                character.isGrounded = false;
                if(Falling(0) && rb.velocity.y < MaxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, MaxFallSpeed);
                }
            }
        }
    }
}