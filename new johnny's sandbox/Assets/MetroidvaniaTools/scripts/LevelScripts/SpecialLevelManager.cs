using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{

    public class SpecialLevelManager : Managers
    {
        [SerializeField]
        private float buoyancyForce ;
        [SerializeField]
        private float updraftForce;

        protected Jump jump;
        
        protected override void Initialization()
        {
            base.Initialization();
            jump = player.GetComponent<Jump>();
        }
        
        protected virtual void OnTriggerStay2D(Collider2D collision)//触れてる間のものは全部ここ
        {
            if (tag == "Water" && collision.gameObject.tag == "Player")
            {
                jump.gravityForceMultiplier = buoyancyForce;
                updraftForce = jump.glideGravity;
            }
            if(tag == "Updraft" && collision.gameObject.tag=="Player")
            {
                jump.glideGravity = updraftForce;
                buoyancyForce = jump.gravityForceMultiplier;
            }
            if (tag == "Space" && collision.gameObject.tag == "Player")
            {
                jump.glideGravity = updraftForce;
                jump.gravityForceMultiplier = buoyancyForce;
            }


        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<Jump>().glideGravity = Jump.originglideGravity;
                collision.gameObject.GetComponent<Jump>().gravityForceMultiplier = Jump.originGravityMultiplier;
            }
        }

    }
}