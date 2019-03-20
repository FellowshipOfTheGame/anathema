using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Chase : Anathema.Fsm.CleaningRobotState
    {
        //[Tooltip("The max distance that the player is considered lost.")]
        //[SerializeField] float distLostPlayer;

        [Tooltip("Speed in which the robot is able to move.")]
        [SerializeField] float speed;

        [Tooltip("Here goes the amount and the GameObjects which limits the chasing area. The spots must be in the ground. In the prefab corresponds the blue spots.")]
        [SerializeField] Transform[] chaseSpots;

        [SerializeField] float rayGroundMaxDist;

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
            if (player != null)
            {
                playerDist = player.position - transform.position;
                Flip();
                RaycastGroundCheck();
                Chasing();

                if (CheckPlayer() == false)
                {
                    animator.SetBool("isPatrolling", true);
                    animator.SetBool("isChasing", false);
                    fsm.Transition<Patrol>();

                }
                else if (RaycastUpdate() == true && CheckPlayer() == true && playerDist.magnitude <= 2.5f)
                {
                    myrBody.velocity = Vector3.zero;
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
            if (playerDist.magnitude > 1f)
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
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Checks if the robot is walking in the ground
        /// </summary>
        private void RaycastGroundCheck()
        {
            Debug.DrawRay(transform.position, Vector2.down, Color.red);
            RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, rayGroundMaxDist, LayerMask.GetMask("Ground"));

            if (groundInfo.collider == false)
            {
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
                fsm.Transition<Patrol>();
            }
        }

        /// <summary>
        /// This method checks if the player is in the chasing area, which is the space between the two chaseSpots.
        /// </summary>
        /// <returns></returns>
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
        /// This function flips the robot, according to the direction which it's heading and the Sprite flipping
        /// </summary>
        private void Flip()
        {
           // float distY = player.position.y - transform.position.y;
            if (CheckPlayer() == false)
            {
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
                fsm.Transition<Patrol>();
            }
            else if (RaycastUpdate() == false && CheckPlayer() == true)
            {
                if (sRenderer.flipX == false)
                {
                    sRenderer.flipX = true;
                }
                else
                {
                    sRenderer.flipX = false;
                }
            }


        }

        public override void Exit() { }

    }
}