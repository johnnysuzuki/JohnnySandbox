using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class CameraFollow : Managers
    {
        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void FixedUpdate()
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
    }
}