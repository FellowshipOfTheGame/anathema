using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.WallRobot
{
    public class Right : Anathema.Fsm.WallRobotState
    {
        public override void Enter() { }

        /// <summary>
        /// In this class, FixedUpdate is used to make the robot move. 
        /// </summary>
        private void FixedUpdate()
        {
            if (sRenderer.flipX == false)
            {
                if (platformsWalkingBot == true)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (wallsWalkingBot == true)
                {
                    Patrolling();
                }
            }
            else if (sRenderer.flipX == true)
            {
                if (platformsWalkingBot == true)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
                else if (wallsWalkingBot == true)
                {
                    Patrolling();
                }

            }

            right = RightRaycast();
            up = UpRaycast();
            down = DownRaycast();

            if (spotControl.platform == false)
            {
                CheckRaycasts();
            }
            else if (spotControl.platform == true)
            {
                Invoke("CheckRaycasts", 0.4f);
            }
        }

        //-----------------------------------------------------------------------------------------------//
        /*Each of these methods cast a ray, to three different directions, that are used to check for walls and ground, so that
            the robot can know which direction it needs to go, depending if the ratcast is true or not.
            Also, the raycasts, change depending on the Sprite flipping.
         */
        private RaycastHit2D RightRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            if (sRenderer.flipX == false)
            {

                Debug.DrawRay(startPos + (Vector2.up * -0.5f), Vector2.right, Color.red);
                return Physics2D.Raycast(startPos + (Vector2.up * -0.5f), Vector2.right, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }
            else
            {
                Debug.DrawRay(startPos + (Vector2.up * 0.5f), Vector2.right, Color.red);
                return Physics2D.Raycast(startPos + (Vector2.up * 0.5f), Vector2.right, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }

        }

        private RaycastHit2D UpRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            if (sRenderer.flipX == false)
            {
                Debug.DrawRay(startPos + (Vector2.left * -0.6f), Vector2.up, Color.green);
                return Physics2D.Raycast(startPos + (Vector2.left * -0.6f), Vector2.up, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }
            else
            {
                Debug.DrawRay(startPos + (Vector2.left * 0.6f), Vector2.up, Color.green);
                return Physics2D.Raycast(startPos + (Vector2.left * 0.6f), Vector2.up, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }
        }

        private RaycastHit2D DownRaycast()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
            if (sRenderer.flipX == false)
            {
                Debug.DrawRay(startPos + (Vector2.left * 0.6f), Vector2.down, Color.black);
                return Physics2D.Raycast(startPos + (Vector2.left * 0.6f), Vector2.down, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }
            else
            {
                Debug.DrawRay(startPos + (Vector2.left * -0.6f), Vector2.down, Color.black);
                return Physics2D.Raycast(startPos + (Vector2.left * -0.6f), Vector2.down, rayWallMaxDist, LayerMask.GetMask("Wall", "Ground"));
            }
        }
        //------------------------------------------------------------------------------------------------//

        /// <summary>
        /// This method check the raycasts above to set the state which it will change.
        /// </summary>
        private void CheckRaycasts()
        {
            //the right raycast is true so it is walking on the right wall
            if (right == true)
            {
                //is going up
                if (sRenderer.flipX == false)
                {
                    if (up == true)
                    {
                        //will go left on the ceiling
                        sRenderer.transform.eulerAngles = new Vector3(0, 0, 180);
                        spotControl.platform = false;
                        fsm.Transition<Up>();
                    }

                }
                //is goin down
                else if (sRenderer.flipX == true)
                {
                    if (down == true)
                    {
                        //will go left on the ground
                        sRenderer.transform.eulerAngles = new Vector3(0, 0, 0);
                        spotControl.platform = false;
                        fsm.Transition<Down>();
                    }

                }
            }

            //the right raycast is now false, so the robot is probably walking on a platform
            else if (right == false)
            {
                if (sRenderer.flipX == false)
                {
                    //will go right on the top edge
                    sRenderer.transform.eulerAngles = new Vector3(0, 0, 0);
                    spotControl.platform = true;
                    fsm.Transition<Down>();
                }
                else if (sRenderer.flipX == true)
                {
                    //will go right on the bottom edge
                    sRenderer.transform.eulerAngles = new Vector3(0, 0, 180);
                    spotControl.platform = true;
                    fsm.Transition<Up>();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Ground") && !other.CompareTag("Wall"))
            {
                transform.Translate(Vector3.zero);
            }
            if (other.CompareTag("Player"))
            {
                Debug.Log("Attack");
                other.transform.GetComponent<Health>().Hp -= damage;
            }
        }


        /// <summary>
        /// If the robot is near the spot, this function updates the next spot, flips if necessary and switches the state.
        /// Otherwise it just makes the robot move in the correct direction (depending on the curret scale) of the spot.
        /// </summary>
        private void Patrolling()
        {
            Vector2 spotDir = moveSpots[spotControl.currentSpot].position - transform.position;

            if (spotDir.magnitude < 1)
            {
                if (spotControl.currentSpot + 1 < moveSpots.Length)
                {
                    spotControl.currentSpot++;
                }
                else
                {
                    spotControl.currentSpot = 0;
                }

                if (sRenderer.flipX == false)
                {
                    sRenderer.flipX = true;
                }
                else if (sRenderer.flipX == true)
                {
                    sRenderer.flipX = false;
                }
            }
            else
            {
                if (sRenderer.flipX == false)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (sRenderer.flipX == true)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);

                }
            }
        }

        public override void Exit() { }
    }
}