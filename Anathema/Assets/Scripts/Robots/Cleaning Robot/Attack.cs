using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Attack : Anathema.Fsm.CleaningRobotState
    {
        [SerializeField] float maxDist;
        [SerializeField] float miniDist;
        [SerializeField] int damage;

        /// <summary>
        /// In this class, the Enter is used to find and get the Transform of the player
        /// </summary>
        public override void Enter()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        /// <summary>
        /// In this class, Fixed Update is used to switch out of movement states.
        /// </summary>
        void FixedUpdate()
        {
            if (player != null)
            {
                playerDist = player.position - transform.position;

                if (RaycastUpdate() == false && playerDist.magnitude > maxDist)
                {
                    animator.SetBool("isPatrolling", true);
                    fsm.Transition<Patrol>();
                }
                else if (playerDist.magnitude >= miniDist)
                {
                    animator.SetBool("isChasing", true);
                    fsm.Transition<Chase>();
                }

            }
            else
            {
                Debug.LogWarning("Attack - Cleaning Robot: player is missing");
                fsm.Transition<CIdle>();
            }
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

            return Physics2D.Raycast(startPos, direction, raycastMaxDist, LayerMask.GetMask("Player"));
        }

        /// <summary>
        /// Checks if the player is near/ in the line of sight of the robot based on the raycast. Also changes the direction
        /// of the raycast according to the direction of the robot.
        /// </summary>
        private bool RaycastUpdate()
        {
            Vector2 direction = new Vector2(1, 0);
            if (sRenderer.flipX == true)
            {
                direction *= -1;
            }

            hit = CheckRaycast(direction);

            if (hit.collider)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    if(playerDist.magnitude < miniDist)
                    {
                        //hit.transform.GetComponent<Health>().Hp-=damage;
                        Debug.Log("Damaging Player");
                       fsm.Transition<KnockBack>();
                    }
                }
                    return true;
            }

            return false;

        }
        public override void Exit() { }
    }

}



