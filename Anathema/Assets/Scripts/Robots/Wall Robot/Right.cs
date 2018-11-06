using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.WallRobot
{
    public class Right : Anathema.Fsm.WallRobotState
    {

        /// <summary>
        /// In this class, Enter is used to set the direction of the raycasts 
        /// </summary>
        public override void Enter()
        {
            direction = Vector2.right;

            if (platformsWalkingBot == true)
            {
                if (transform.localScale.x > 0)
                {
                    groundDetectionDir = Vector2.down;
                }
                else if (transform.localScale.x < 0)
                {
                    groundDetectionDir = Vector2.up;
                }
            }
            else if (wallsWalkingBot == true)
            {
                spotControl = GetComponent<SpotControl>();

            }
        }

        /// <summary>
        /// In this class, FixedUpdate is used to set the direction of the movement depending on the scale of the robot and switch the state if it's a platformbot, and 
        /// call the movement functions if it's a wallbot.
        /// </summary>
        void FixedUpdate()
        {
            if (platformsWalkingBot == true)
            {
                if (transform.localScale.x > 0)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (transform.localScale.x < 0)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }

                //Needs to wait some time to call the RaycastGroundCheck function or it won't wait until the hole robot has turned
                Invoke("RaycastGroundCheck", 0.4f);
                wallInfo = RaycastWallCheck();

               /* if (wallInfo.collider)
                {
                    if (wallInfo.collider.CompareTag("Wall"))
                    {
                         Debug.Log("Achou uma parede: Right");
                        Flip();
                        fsm.Transition<Left>();
                    }
                }*/
            }
            else if (wallsWalkingBot == true)
            {
                wallInfo = RaycastWallCheck();
                Patrolling();
                RotationSide();
            }

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Ground") && !other.collider.CompareTag("Wall"))
            {
                transform.Translate(Vector3.zero);
            }
            if (other.collider.CompareTag("Player"))
            {
                 Debug.Log("Hit the Player");
                //col.transform.GetComponent<Health>().OnHit();
            }
        }
        /// <summary>
        /// Gets the information of the raycast which checks the ground and if there is no ground switch the state depending on the current scale of the robot
        /// </summary>
        private void RaycastGroundCheck()
        {
            Debug.DrawRay(groundDetection.position, groundDetectionDir, Color.red);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, groundDetectionDir, rayGroundMaxDist);
            if (groundInfo.collider == false)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
                if (transform.localScale.x > 0)
                {
                    fsm.Transition<Down>();
                }
                else if (transform.localScale.x < 0)
                {
                    fsm.Transition<Up>();
                }
            }
        }


        /// <summary>
        /// Checks if the "Wall raycast" found a wall, if so switches the state depending on the current scale of the robot
        /// </summary>
        private void RotationSide()
        {
            if (wallInfo.collider == true && (wallInfo.collider.CompareTag("Wall")))
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
                if (transform.localScale.x > 0)
                {
                    fsm.Transition<Up>();
                }
                else if (transform.localScale.x < 0)
                {
                    fsm.Transition<Down>();
                }
            }
        }

        /// <summary>
        /// Gets information about the raycast that looks for walls
        /// </summary>
        private RaycastHit2D RaycastWallCheck()
        {
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);

            Debug.DrawRay(startPos, direction, Color.red);
            return Physics2D.Raycast(startPos, direction, rayWallMaxDist);
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

                Flip();
                fsm.Transition<Left>();
            }
            else
            {
                if (transform.localScale.x > 0)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (transform.localScale.x < 0)
                {
                    transform.Translate(Vector2.left * speed * Time.deltaTime);

                }
            }
        }

        /// <summary>
        /// Flips the robot by changing it's scale
        /// </summary>
        private void Flip()
        {
            currentScale = transform.localScale;
            currentScale.x *= -1;
            transform.localScale = currentScale;
        }

      
        public override void Exit() { }
    }
}