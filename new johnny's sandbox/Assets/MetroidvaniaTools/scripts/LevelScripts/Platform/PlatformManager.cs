using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MetroidvaniaTools
{
    [RequireComponent(typeof(BoxCollider2D))]

    public class PlatformManager : Managers
    {
        [SerializeField]
        protected float timeTillDoSomething;
        [SerializeField]
        protected float timeFalling;
        [SerializeField]
        protected float timeTillReset;
        [SerializeField]
        protected float destroyPlatform;

        protected BoxCollider2D platformCollider;
        protected Rigidbody2D platformRB;
        protected Vector3 originalPlatformPosition;
        protected float currentTimeTillDoSomething;
        protected float currentTimeFalling;
        protected LayerMask playerLayer;

        protected override void Initialization()
        {
            base.Initialization();
            currentTimeTillDoSomething = timeTillDoSomething;
            currentTimeFalling = timeFalling;
            originalPlatformPosition = transform.position;
            platformCollider = GetComponent<BoxCollider2D>();
            platformRB = GetComponent<Rigidbody2D>();
            playerLayer = player.layer;
        }

        protected virtual void FixedUpdate()
        {
            CollisionCheck();
        }

        protected virtual bool CollisionCheck()
        {
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y), new Vector2(platformCollider.bounds.size.x - .1f, .05f), 0, Vector2.up, .05f, playerLayer);
            if (hit)
            {
                return true;
            }
            return false;
        }

    }
}