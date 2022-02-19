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
    }
}