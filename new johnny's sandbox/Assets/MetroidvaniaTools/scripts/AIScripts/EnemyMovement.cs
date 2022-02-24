using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyMovement : AIManagers
    {
        [SerializeField]
        protected enum MovementType { Normal }
        [SerializeField]
        protected MovementType type;
        [SerializeField]
        protected bool spawnFacingLeft;
        [SerializeField]
        protected bool turnAroundOnCollision;
        [SerializeField]
        protected bool avoidFalling;
        [SerializeField]
        protected float timeTillMaxSpeed;
        [SerializeField]
        protected float MaxSpeed;//最高速
        [SerializeField]
        protected LayerMask collidersToTurnAroundOn;

        private bool spawning = true;
        private float acceleration;//加速度
        private float currentSpeed;//現在の速度
        private float direction;//向き
        private float runTime;//走っている時間
        private float originalGravityForceMultiplier;//最初の重力

        protected bool wait;
        protected float originalWaitTime = .1f;
        protected float currentWaitTime;

        protected override void Initialization()
        {
            base.Initialization();
            if (spawnFacingLeft)
            {
                enemyCharacter.facingLeft = true;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            currentWaitTime = originalWaitTime;
            Invoke("Spawning", .01f);
        }

        protected virtual void FixedUpdate()
        {
            Movement();
            CheckGround();
            EdgeOfFloor();
            HandleWait();
        }

        protected virtual void Movement()
        {
            if (!enemyCharacter.facingLeft)
            {
                direction = 1;
                if (CollisionCheck(Vector2.right, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = true;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            else
            {
                direction = -1;
                if (CollisionCheck(Vector2.left, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = false;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            acceleration = MaxSpeed / timeTillMaxSpeed;
            runTime += Time.deltaTime;
            currentSpeed = direction * acceleration * runTime;
            CheckSpeed();
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        protected virtual void CheckSpeed()
        {
            if(currentSpeed > MaxSpeed)
            {
                currentSpeed = MaxSpeed;
            }
            if(currentSpeed < -MaxSpeed)
            {
                currentSpeed = -MaxSpeed;
            }
        }

        protected virtual void EdgeOfFloor()
        {
            if(rayHitNumber == 1 && avoidFalling && !wait)
            {
                currentWaitTime = originalWaitTime;
                wait = true;
                enemyCharacter.facingLeft = !enemyCharacter.facingLeft;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }

        protected virtual void Spawning()
        {
            spawning = false;
        }

        protected virtual void HandleWait()
        {
            currentWaitTime -= Time.deltaTime;
            if(currentWaitTime <= 0)
            {
                wait = false;
                currentWaitTime = 0;
            }
        }
    }
}