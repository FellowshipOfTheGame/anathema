using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Patrol : Anathema.Fsm.CleaningRobotState
    {
        [Tooltip("Speed in which the robot is able to move.")]
        [SerializeField] float speed = 5f;

        [Tooltip("Here goes the amount and the GameObjects which limits the robot patrol area. The spots must be in the ground.")]
        [SerializeField] Transform[] moveSpots;

        [Tooltip("Here goes the amount and the GameObjects which limits the chasing area. The spots must be in the ground.")]
        [SerializeField] Transform[] chaseSpots;

        //Stores the spot which the robot is heading
        private int currentSpot;

        //Stores the current position of the robot
        private Vector2 robotPos;

        //Stores the position of the spot which the robot is heading
        private Vector2 nextPos;

        private Vector2 distBetweenSpots;

        /// <summary>
        /// In this class, the Enter is used to find the player and get it's Transform component
        ///  and initializes the variables. Also flip the robot if necessary.
        /// </summary>
        public override void Enter()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            currentScale = transform.localScale;
            currentSpot = 0;
            distBetweenSpots = chaseSpots[0].position - chaseSpots[1].position;
            robotPos = transform.position;
            nextPos = moveSpots[currentSpot].position;
            Flip();
        }

        /// <summary>
        /// In this class, the FixedUpdate calls functions that make the robot move and look for the player, updates
        /// the robot position and it's distance to the player. If the player was not found in the Enter, changes
        /// the movement state.
        /// </summary>
        void FixedUpdate()
        {
            if (player != null)
            {
                playerDist = player.position - transform.position;
                robotPos = transform.position;
                Patrolling();
                FindPlayer();
            }
            else
            {
                Debug.Log("Patrol - Cleaning Robot: Player is missing");
                animator.SetBool("isPatrolling", false);

                fsm.Transition<CIdle>();

            }
        }

        /// <summary>
        /// If the robot is near the spot, this function updates the next spot and flips if necessary.
        /// Otherwise it just makes the robot move in the direction of the spot. Also updates the 'nextPos' variable.
        /// </summary>
        private void Patrolling()
        {
            Vector2 spotDir = moveSpots[currentSpot].position - transform.position;

            if (spotDir.magnitude < 1)
            {
                if ((currentSpot + 1) < moveSpots.Length)
                {
                    currentSpot++;
                }
                else
                {
                    currentSpot = 0;
                }
                nextPos = moveSpots[currentSpot].position;
                Flip();
            }
            else
            {
                myrBody.velocity = spotDir.normalized * speed;
            }
        }

        /// <summary>
        /// This function changes the state if the player was found
        /// </summary>
        private void FindPlayer()
        {
            if ((RaycastUpdate() == true) || (RaycastUpdate() == false) && CheckPlayer() == true)
            {
                Debug.Log("Player is in the area");
                animator.SetBool("isChasing", true);
                animator.SetBool("isPatrolling", false);
                fsm.Transition<Chase>();
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

            if (nextPos.x < robotPos.x && currentScale.x > 0)
            {

                currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
            }
            else if (nextPos.x > robotPos.x && currentScale.x < 0)
            {

                currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
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

        public override void Exit() { }

    }

}