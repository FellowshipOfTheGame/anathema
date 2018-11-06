using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Chase : Anathema.Fsm.CleaningRobotState
    {
        [Tooltip("The max distance that the player is considered lost.")]
        [SerializeField] float distLostPlayer = 15f;

        [Tooltip("Speed in which the robot is able to move.")]
        [SerializeField] float speed = 10f;

        /// <summary>
        /// In this class, the Enter is used to find and get the Transform of the player
        /// </summary>
        public override void Enter()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        /// <summary>
        /// In this class, Fixed Update is used to switch out of movement states and to call functions that make the robot moves
        /// </summary>
        void FixedUpdate()
        {
            currentScale = transform.localScale;
            if (player != null)
            {
                playerDist = player.position - transform.position;
                Flip();
                Chasing();

                if (RaycastUpdate() == false && playerDist.magnitude > distLostPlayer)
                {
                    animator.SetBool("isPatrolling", true);
                    animator.SetBool("isChasing", false);
                    fsm.Transition<Patrol>();

                }
                else if (RaycastUpdate() == true && playerDist.magnitude <= 1.8f)
                {
                    fsm.Transition<Attack>();
                }
            }
            else
            {
                Debug.Log("Chase - Cleaning Robot: Player is missing");
                animator.SetBool("isChasing", false);
                fsm.Transition<CIdle>();

            }
        }

        /// <summary>
        /// Makes the robot moves in the direction of the player if it's still far.false Also sets the velocity to zero if its near the player
        /// </summary>
        void Chasing()
        {
            if (playerDist.magnitude > 1.8)
                myrBody.velocity = playerDist.normalized * speed;
            else
                myrBody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Gets the raycast which is used to look for the player
        /// </summary>
        /// <param name="direction"> Direction provided by the RaycastUpdate function</param>
        /// <returns></returns>
        private RaycastHit2D CheckRaycast(Vector2 direction)
        {
            float dirOriginOffset = origindir * (direction.x > 0 ? 1 : -1);
            Vector2 startPos = new Vector2(transform.position.x + dirOriginOffset, transform.position.y);

            Debug.DrawRay(startPos, direction, Color.red);

            return Physics2D.Raycast(startPos, direction, raycastMaxDist);
        }


        /// <summary>
        /// Checks if the player is near/ in the line of sight of the robot based on the raycast. Also changes the direction
        /// of the raycast according to the direction of the robot.
        /// </summary>
        private bool RaycastUpdate()
        {
            Vector2 direction = new Vector2(1, 0);
            if (transform.localScale.x < 0)
            {
                direction *= -1;
            }
            hit = CheckRaycast(direction);

            if (hit.collider)
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// This function flips the robot, according to the direction which it's heading and the current scale 
        /// </summary>
        private void Flip()
        {
            float distY = player.position.y - transform.position.y;
            if (distY > 1.5f)
            {
                transform.localScale = currentScale;
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
                fsm.Transition<Patrol>();
            }
            else if (RaycastUpdate() == false && (playerDist.magnitude <= 10f))
            {
                currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }

        }



        public override void Exit() { }

    }
}