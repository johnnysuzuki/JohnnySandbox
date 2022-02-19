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
        protected float timeTillMaxSpeed;
        [SerializeField]
        protected float MaxSpeed;//最高速

        private float acceleration;//加速度
        private float currentSpeed;//現在の速度
        private float direction;//向き
        private float runTime;//走っている時間
        private float originalGravityForceMultiplier;//最初の重力

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        protected virtual void Movement()
        {
            if (!enemyCharacter.facingLeft)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
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
    }
}