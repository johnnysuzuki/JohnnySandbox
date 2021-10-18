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
        [SerializeField]
        protected float glideTime;
        [SerializeField]
        public float glideGravity;//グライド用の倍数
        [SerializeField]
        public float gravityForceMultiplier;//重力用の倍数


        public static float originglideGravity;　//最初にキャラが持ってるグライド用重力
        public static float originGravityMultiplier; //最初にキャラが持ってる重力係数

        public LayerMask collisionLayer;//何処とぶつかるか

        public static float gravity = -19.6f;//基礎重力

        private bool isJumping;//ジャンプ中か否か
        private float jumpCountDown;
        private float fallCountDown;
        private int NumberOfJumpsLeft;
        private float JumpCountDown;

        protected override void Initialization()
        {
            base.Initialization();
            NumberOfJumpsLeft = MaxJumpNum;
            JumpCountDown = ButtonHoldTime;
            fallCountDown = glideTime;
            originglideGravity = glideGravity;
            originGravityMultiplier = gravityForceMultiplier;
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
                    fallCountDown = glideTime;
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
            Gliding();
            CharacterFalling();
        }

        protected virtual void CharacterFalling()
        { 
            rb.AddForce(Vector2.up * gravity * gravityForceMultiplier);
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

        protected virtual void Gliding()
        {
            if(JumpHeld())
            {
                if (glideGravity < 0)
                {
                    if (fallCountDown > 0 && rb.velocity.y > AcceptedFallSpeed)
                    {
                        FallSpeed(glideGravity);
                        return;
                    }
                }

                else if (Falling(0))
                {
                    fallCountDown -= Time.deltaTime;
                    if (fallCountDown > 0 && rb.velocity.y > AcceptedFallSpeed)
                    {
                        FallSpeed(glideGravity);
                    }
                }
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
                fallCountDown = glideTime;
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