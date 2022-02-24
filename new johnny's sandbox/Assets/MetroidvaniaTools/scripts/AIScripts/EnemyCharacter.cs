using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyCharacter : MonoBehaviour
    {
        [HideInInspector]
        public bool facingLeft;
        protected Rigidbody2D rb;
        protected Collider2D col;
        protected int rayHitNumber;


        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
        }

        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void CheckGround()
        {
            Ray2D[] groundRays = new Ray2D[3];
            groundRays[0].origin = new Vector2(transform.position.x - (transform.localScale.x * .5f), transform.position.y - .05f);
            groundRays[1].origin = new Vector2(transform.position.x , transform.position.y - .05f);
            groundRays[2].origin = new Vector2(transform.position.x + (transform.localScale.x * .5f), transform.position.y - .05f);
            RaycastHit2D[] hits = new RaycastHit2D[3];
            for(int i = 0; i<3; i++)
            {
                hits[i] = Physics2D.Raycast(groundRays[i].origin, -transform.up, Mathf.Abs(transform.localScale.x * .5f));
            }
            int numberOfHits = 0;
            foreach(RaycastHit2D hit in hits)
            {
                if (hit)
                {
                    numberOfHits++;
                }
            }
            rayHitNumber = numberOfHits;
        }
    }
}