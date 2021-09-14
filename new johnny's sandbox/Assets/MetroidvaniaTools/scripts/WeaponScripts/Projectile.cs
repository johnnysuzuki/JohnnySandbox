using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected WeaponTypes weapon;

        public bool fired;

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        public virtual void Movement()
        {
            if (fired)
            {

            }
        }

    }
}