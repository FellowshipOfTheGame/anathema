using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Attack : Anathema.Fsm.CleaningRobotState
    {
       // [Tooltip("The maximum distance to the player be considered lost.")]
       // [SerializeField] float maxDist;
        [Tooltip("The minimum distance between the robot and the player so it will keep chasing the player.")]
        [SerializeField] float miniDist;

        [Tooltip("attack damage of the cleaning robot.")]
        [SerializeField] int damage;
        
         [Tooltip("Here goes the amount and the GameObjects which limits the chasing area. The spots must be in the ground. In the prefab corresponds the red spots.")]
        [SerializeField] Transform[] chaseSpots;

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
                RaycastUpdate();
                if (CheckPlayer() == false)
                {
                    animator.SetBool("isPatrolling", true);
                    fsm.Transition<Patrol>();
                }
                else if (playerDist.magnitude >= miniDist && CheckPlayer() == true)
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

        private bool CheckPlayer()
        {
            Collider2D hits = Physics2D.OverlapArea(chaseSpots[0].position, chaseSpots[1].position, LayerMask.GetMask("Player"));
            Debug.DrawLine(chaseSpots[0].position, chaseSpots[1].position, Color.blue);
            if (hits)
            {

                if (hits.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;

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
                    if (playerDist.magnitude < miniDist)
                    {
                        Debug.Log("Attack");
                        Vector2 hitVector = hit.transform.position - transform.position;
                        hit.transform.GetComponent<Health>().Damage(damage, hitVector, Health.DamageType.EnemyAttack);
                        //fsm.Transition<KnockBack>();
                    }
                }
                return true;
            }

            return false;

        }
        
        public override void Exit() { }
    }

}



