using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Dash : Abilities
    {
        [SerializeField]
        protected float DashForce;
        [SerializeField]
        protected float DashCooldownTime;
        [SerializeField]
        protected float DashAmountTime;

        [SerializeField]
        protected LayerMask DashingLayers;

        private bool canDash;
        private float DashCountDown;
        private Vector2 deltaPosition;

        // Update is called once per frame
        protected virtual void Update()
        {
            DashPressed();
        }

        protected virtual bool DashPressed()
        {
            if (Input.GetButtonDown("Dash")&& canDash)
            {
                Dashing();
                return true;
            }
            else
                return false;
        }

        protected virtual void Dashing()
        {
            deltaPosition = transform.position;
            DashCountDown = DashCooldownTime;
            character.isDashing = true;
            StartCoroutine(FinishedDashing());
        }

        protected virtual void FixedUpdate()
        {
            DashMode();
            ResetDashCounter();
        }

        protected virtual void DashMode()
        {
            if (character.isDashing)
            {
                FallSpeed(0);
                movement.enabled = false;
                aimManager.enabled = false;
                if (!character.isFacingLeft)
                {
                    DashCollision(Vector2.right,.5f,DashingLayers);
                    rb.velocity = new Vector2(DashForce,0);
                }
                else
                {
                    DashCollision(Vector2.left, .5f, DashingLayers);
                    rb.velocity = new Vector2(-DashForce, 0);
                }
            }
        }


        protected virtual void DashCollision(Vector2 direction, float distance, LayerMask collision)
        {
            Debug.DrawRay(transform.position, direction, Color.green);
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    hits[i].collider.enabled = false;
                    StartCoroutine(TurnColliderBackOn(hits[i].collider.gameObject));
                }
            }
        }

        protected virtual void ResetDashCounter()
        {
            if (DashCountDown > 0)
            {
                canDash = false;
                DashCountDown -= Time.deltaTime;
            }
            else
                canDash = true;
        }

        protected virtual IEnumerator FinishedDashing()
        {
            yield return new WaitForSeconds(DashAmountTime);
            character.isDashing = false;
            movement.enabled = true;
            aimManager.enabled = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            RaycastHit2D[] hits = new RaycastHit2D[10];
            yield return new WaitForSeconds(.1f);
            hits = Physics2D.CapsuleCastAll(new Vector2(col.bounds.center.x, col.bounds.center.y + .05f), new Vector2(col.bounds.size.x, col.bounds.size.y - .1f), CapsuleDirection2D.Vertical, 0, Vector2.zero, 0, jump.collisionLayer);
            if(hits.Length > 0)
            {
                transform.position = deltaPosition;
            }


        }

        protected virtual IEnumerator TurnColliderBackOn(GameObject obj)
        {
            yield return new WaitForSeconds(DashAmountTime);
            obj.GetComponent<Collider2D>().enabled = true;
        }
    }
}