using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Hook : MonoBehaviour
    {

        protected GameObject player;
        protected GrapplingHook grapplingHook;
        protected Weapon weapon;
        [SerializeField]
        protected LayerMask layers;

        protected virtual void Start()
        {
            player = GameObject.FindWithTag("Player");
            grapplingHook = player.GetComponent<GrapplingHook>();
            weapon = player.GetComponent<Weapon>();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if((1 << collision.gameObject.layer & layers) != 0 && !grapplingHook.connected)
            {
                grapplingHook.connected = true;
                grapplingHook.objectConnectedTo = weapon.currentProjectile.transform.position;
                weapon.currentProjectile.GetComponent<HingeJoint2D>().enabled = true;
                weapon.currentProjectile.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
            }
        }

    }
}