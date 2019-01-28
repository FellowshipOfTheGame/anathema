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

        [Tooltip("Here goes the amount and the GameObjects which limits the chasing area. The spots must be in the ground.")]
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
            currentScale = transform.localScale;
            if (player != null)
            {
                playerDist = player.position - transform.position;
                Flip();
                RaycastGroundCheck();
                Chasing();

                if (RaycastUpdate() == false && playerDist.magnitude > distLostPlayer)
                {
                    animator.SetBool("isPatrolling", true);
                    animator.SetBool("isChasing", false);
                    Debug.Log("FixedUpdate: Foi pro patrol");
                    fsm.Transition<Patrol>();

                }
                else if (RaycastUpdate() == true && playerDist.magnitude <= 1.5f)
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
        private void RaycastGroundCheck()
        {
            Debug.DrawRay(transform.position, Vector2.down, Color.red);
            RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, rayGroundMaxDist, LayerMask.GetMask("Ground"));

            if (groundInfo.collider == false)
            {
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
                Debug.Log("RaycastGroundCheck: Foi pro patrol");
                fsm.Transition<Patrol>();
            }
        }
        private bool CheckPlayer()
        {
            Collider2D hits = Physics2D.OverlapArea(chaseSpots[0].position, chaseSpots[1].position, LayerMask.GetMask("Player"));
            Debug.DrawLine(chaseSpots[0].position, chaseSpots[1].position, Color.blue);
            //Collider2D hit = Physics2D.OverlapBox(new Vector2(distBetweenSpots.x/2, chaseSpots[1].position.y), distBetweenSpots, LayerMask.GetMask("Player"));
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
                Debug.Log("Flip: Foi pro patrol");
                fsm.Transition<Patrol>();
            }
            else if (RaycastUpdate() == false && (playerDist.magnitude <= 7f) && CheckPlayer() == true)
            {
                currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }

        }

        public override void Exit() { }

    }
}