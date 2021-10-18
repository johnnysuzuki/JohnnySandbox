using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class GrapplingHook : Abilities
    {
        [SerializeField]
        protected float hookLength;
        [SerializeField]
        protected float minHookLength;
        [SerializeField]
        protected float hookReelSpeed;
        [SerializeField]
        protected float verticalForce;
        [SerializeField]
        protected float horizontalForce;

        public bool connected;
        public bool removed;
        public Vector2 objectConnectedTo;
        public GameObject hookTrail;

        public float distanceFromHookedObject;
        public bool canDrawLine;
        public bool grappling;

        protected override void Initialization()
        {
            base.Initialization();
            hookTrail.SetActive(false);
            Invoke("MaybeDisable", .01f);
            if(weapon.currentWeapon != null && weapon.currentWeapon.projectile.tag != "GrapplingHook")
            {
                enabled = false;
            }
        }

        protected virtual void FixedUpdate()
        {
            GrappleFired();
            RemoveGrapple();
        }

        protected virtual void GrappleFired()
        {
            if(Input.GetButton("Fire1") && weapon.currentProjectile != null && weapon.currentProjectile.GetComponent<Projectile>().fired && weapon.currentProjectile.tag == "GrapplingHook")
            {
                distanceFromHookedObject = Vector2.Distance(weapon.gunBarrel.position, weapon.currentProjectile.transform.position);
                canDrawLine = true;
                Invoke("DrawLine", .1f);
                if(distanceFromHookedObject > hookLength)
                {
                    canDrawLine = false;
                    DrawLine();
                    weapon.currentProjectile.GetComponent<Projectile>().DestroyProjectile(); 
                }
            }
            else
            {
                canDrawLine = false;
                DrawLine();
            }
            if (connected)
            {
                GrappleHanging();
            }
        }

        protected virtual void DrawLine()
        {
            if (canDrawLine)
            {
                hookTrail.SetActive(true);
                hookTrail.transform.position = weapon.gunBarrel.position;
                hookTrail.transform.rotation = weapon.gunRotation.rotation;
                hookTrail.GetComponent<SpriteRenderer>().size = new Vector2(distanceFromHookedObject, .64f);
            }
            else
            {
                distanceFromHookedObject = 0;
                hookTrail.GetComponent<SpriteRenderer>().size = new Vector2(0, .64f);
                hookTrail.SetActive(false);
                if(weapon.currentProjectile != null && weapon.currentProjectile.tag == "GrapplingHook")
                {
                    weapon.currentProjectile.GetComponent<Projectile>().DestroyProjectile();
                }
            }

        }

        protected virtual void GrappleHanging()
        {
            rb.freezeRotation = false;
            float step = hookReelSpeed * Time.deltaTime;
            grappling = true; //グラップル中はエイム方向を一定に定める
            aimManager.whereToAim.transform.position = objectConnectedTo;
            weapon.currentProjectile.transform.position = objectConnectedTo;
            weapon.currentProjectile.GetComponent<Projectile>().projectileLifeTime = weapon.currentWeapon.lifeTime;
            //weapon.currentProjectile.transform.position = objectConnectedTo.transform.position;
            distanceFromHookedObject = Vector2.Distance(weapon.gunBarrel.position, objectConnectedTo);
            transform.rotation = Quaternion.FromToRotation(Vector2.up, new Vector3(objectConnectedTo.x,objectConnectedTo.y,0) - transform.position);

            if (Input.GetButton("Up") && distanceFromHookedObject >= minHookLength)
            {
                transform.position = Vector2.MoveTowards(transform.position, objectConnectedTo, step);

            }
            if (Input.GetButton("Down") && distanceFromHookedObject < hookLength - .5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, objectConnectedTo, -1*step);
            }

        }

        public virtual void RemoveGrapple()
        {
            if (!Input.GetButton("Fire1") || removed)
            {
                removed = true;
                grappling = false;
                if (connected)
                {
                    connected = false;
                    objectConnectedTo = new Vector2 (0f,0f);
                    rb.AddForce(Vector2.up * verticalForce);
                    if (!character.isFacingLeft)
                    {
                        rb.AddForce(Vector2.right * horizontalForce);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * horizontalForce);
                    }
                    StartCoroutine(DisableMovement());
                }
                if(weapon.currentProjectile != null)
                {
                    weapon.currentProjectile.transform.position = weapon.gunBarrel.position;
                    weapon.currentProjectile.GetComponent<Projectile>().DestroyProjectile();
                }
                ReturnHook();
            }
        }

        protected virtual void MaybeDisable()
        {
            if (weapon.currentWeapon != null && weapon.currentWeapon.projectile.tag != "GrapplingHook")
            {
                enabled = false;
            }
        }

        protected virtual void ReturnHook()
        {
            canDrawLine = false;
            connected = false;
            DrawLine();
        }

        protected virtual IEnumerator DisableMovement()
        {
            movement.enabled = false;
            yield return new WaitForSeconds(.1f);
            movement.enabled = true;
        }

    }
}