using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{

    public class Horizontalmovement : Abilities
    {
        [SerializeField]
        protected float timeTillMaxSpeed;
        [SerializeField]
        protected float MaxSpeed;//最高速
        [SerializeField]
        protected float SprintMultiplier;
        [SerializeField]
        protected float hookSpeedMultiplier;
        [SerializeField]
        protected float ladderSpeed;
        [HideInInspector]
        public GameObject currentLadder;

        protected bool above;
        private float acceleration;//加速度
        private float currentSpeed;//現在の速度
        private float horizontalInput;//左右入力
        private float runTime;//走っている時間

        protected override void Initialization()
        {
            base.Initialization();//CharacterのInitializationを呼び出してオーバーライド
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            MovementPressed();
            SprintingHeld();
        }

        //左右入力を検知してboolを返す
        public virtual bool MovementPressed()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
                return true;
            }
            else
                return false;
        }
        //ダッシュボタン入力を検知してboolを返す
        protected virtual bool SprintingHeld()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return true;
            }
            else
                return false;
        }

        protected virtual void FixedUpdate()
        {
            Movement();
            RemoveFromGrapple();
            LadderMovement();
        }

        //移動の処理
        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                acceleration = MaxSpeed / timeTillMaxSpeed;
                runTime += Time.deltaTime;
                currentSpeed = horizontalInput * acceleration * runTime;
                SpeedControl();
            }
            else
            {
                acceleration = 0;
                runTime = 0;
                currentSpeed = 0;
            }
            SpeedMultiplier();
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        protected virtual void RemoveFromGrapple()
        {
            if (grapplingHook.removed)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * 500);
                if(transform.rotation == Quaternion.identity)
                {
                    grapplingHook.removed = false;
                    rb.freezeRotation = true;
                }
            }
        }

        protected virtual void LadderMovement()
        {
            if(character.isOnLadder && currentLadder != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                if(col.bounds.min.y >= (currentLadder.GetComponent<Ladder>().topOfLadder.y - col.bounds.extents.y))
                {
                    above = true;
                }
                else
                {
                    above = false;
                }
                if (Input.GetButton("Up"))
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentLadder.GetComponent<Ladder>().topOfLadder, ladderSpeed * Time.deltaTime);
                    return;
                }
                if (Input.GetButton("Down"))
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentLadder.GetComponent<Ladder>().bottomOfLadder, ladderSpeed * Time.deltaTime);
                    return;
                }

            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            
        }

        //スピードが速くなりすぎないようにする
        protected virtual void SpeedControl()
        {
            if (currentSpeed > 0)
            {
                if (currentSpeed > MaxSpeed)
                {
                    currentSpeed = MaxSpeed;
                }
            }
            if (currentSpeed < 0)
            {
                if (currentSpeed < -MaxSpeed)
                {
                    currentSpeed = -MaxSpeed;
                }
            }
        }

        //ダッシュ時は速度を係数分だけ乗算する
        protected virtual void SpeedMultiplier()
        {
            if (SprintingHeld())
            {
                currentSpeed *= SprintMultiplier;
            }
            if (grapplingHook.connected)
            {

                if (Input.GetButton("Up") || Input.GetButton("Down") || AroundCollisionCheck() || character.isGrounded || rb.transform.position.y > grapplingHook.objectConnectedTo.y)
                {
                    return;
                }
                else
                {
                    currentSpeed *= hookSpeedMultiplier;
                    rb.AddForce(Vector2.up * jump.glideGravity);
                }
                /*
                if(grapplingHook.hookTrail.transform.position.y > grapplingHook.objectConnectedTo.y)
                {
                    currentSpeed *= -hookSpeedMultiplier;
                }
                */
                //rb.rotation -= currentSpeed;
            }
        }

    }
}