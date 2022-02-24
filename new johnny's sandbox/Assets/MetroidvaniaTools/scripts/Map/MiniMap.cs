using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class MiniMap : Managers
    {
        protected virtual void FixedUpdate()
        {
            transform.position = new Vector3(playerIndicator.transform.position.x, playerIndicator.transform.position.y, -10);
        }

    }
}