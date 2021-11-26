using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class MoveablePlatform : PlatformManager
    {
        [SerializeField]
        protected enum PlatformTypes { OneWay,PingPong,StopOnEnd,Loop}
        [SerializeField]
        protected PlatformTypes platformType;
        public List<Vector3> numberOfPaths = new List<Vector3>();
        [SerializeField]
        protected float speed;
        protected bool needsToMove = true;
        protected bool moving;
        protected bool pingPongGoingDown = false;
        protected int currentPath = 0;
        protected int nextPath;

        protected override void Initialization()
        {
            base.Initialization();
            transform.position = numberOfPaths[currentPath];　//本来のコード
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            FindThePath();
            MoveToPosition();
        }


        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if(character.isJumping == false && collision.transform == player.transform && player.transform.position.y > transform.position.y)
            {
                player.transform.parent = transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if(collision.transform == player.transform)
            {
                player.transform.parent = null;
            }
        }

        protected virtual void FindThePath()
        {
            for(int i = currentPath; i < numberOfPaths.Count; i++)
            {
                if (needsToMove)
                {
                    needsToMove = false;
                    if (!pingPongGoingDown)
                    {
                        currentPath = i;
                    }
                    else
                    {
                        currentPath = i - 2;
                    }
                    if(platformType == PlatformTypes.OneWay)
                    {
                        nextPath = i+1;
                        if(nextPath == numberOfPaths.Count)
                        { 
                            nextPath = 0;
                            player.transform.parent = null;
                            transform.position = numberOfPaths[0];
                        }
                    }
                    if(platformType == PlatformTypes.PingPong)
                    {
                        if (!pingPongGoingDown)
                        {
                            nextPath = i+1;
                            if(nextPath == numberOfPaths.Count)
                            {
                                nextPath = i - 2;
                                currentPath--;
                                pingPongGoingDown = true;
                            }
                        }
                        if (pingPongGoingDown)
                        {
                            nextPath = i-1;
                            if (nextPath　<0)
                            {
                                nextPath = 0;
                                currentPath = -1;
                                pingPongGoingDown = false;
                            }
                        }

                    }
                    if(platformType == PlatformTypes.StopOnEnd)
                    {
                        nextPath = i+1;
                        if(nextPath == numberOfPaths.Count)
                        {
                            return;
                        }
                    }
                    moving = true;
                }
            }
        }

        protected virtual void MoveToPosition()
        {
            if (moving)
            {
                if(transform.position == numberOfPaths[nextPath])
                {
                    moving = false;
                    needsToMove = true;
                    currentPath++; 
                }
                if(transform.position == numberOfPaths[nextPath] && currentPath == numberOfPaths.Count)
                {
                    currentPath = 0;
                }
                transform.position = Vector2.MoveTowards(transform.position, numberOfPaths[nextPath], speed * Time.deltaTime);
            }
        }

    }
}