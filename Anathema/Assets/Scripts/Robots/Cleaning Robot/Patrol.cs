using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class Patrol : Anathema.Fsm.CleaningRobotState
    {
        [Tooltip("Speed in which the robot is able to move.")]
        [SerializeField] float speed = 3f;

        [Tooltip("Here goes the amount and the GameObjects which limits the robot patrol area. The spots must be in the ground.")]
        [SerializeField] Transform[] moveSpots;

        [Tooltip("Here goes the amount and the GameObjects which limits the chasing area. The spots must be in the ground. In the prefab corresponds the red spots.")]
        [SerializeField] Transform[] chaseSpots;

        [Tooltip("The max value of the raycast that checks the ground.")]
        [SerializeField] float rayGroundMaxDist;


        //Stores the spot which the robot is heading
        private int currentSpot;

        //Stores the current position of the robot
        private Vector2 robotPos;

        //Stores the position of the spot which the robot is heading
        private Vector2 nextPos;


        /// <summary>
        /// In this class, the Enter is used to find the player and get it's Transform component
        ///  and initializes the variables. Also flip the robot if necessary.
        /// </summary>
        public override void Enter()
        {
            Debug.Log("Entrou no Patrol");
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            currentSpot = 0;
            robotPos = transform.position;
            nextPos = moveSpots[currentSpot].position;
            Flip();
            Debug.Log("PAssou pelo enter");
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
                Debug.Log("Entrou no if do fixedupdate");
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
            Debug.Log("Entrou no patrolling");
            Vector2 spotDir = moveSpots[currentSpot].position - transform.position;
            Debug.Log("Spotdir magnitude" + spotDir.magnitude);
            if (spotDir.magnitude < 1)
            {
                Debug.Log("Entrou no patrolling 1 ");
                if ((currentSpot + 1) < moveSpots.Length)
                {
                    Debug.Log("Entrou no patrolling 2 ");
                    currentSpot++;
                }
                else
                {
                    Debug.Log("Entrou no patrolling 3 "); 
                    currentSpot = 0;
                }
                 Debug.Log("Entrou no patrolling 4 ");
                nextPos = moveSpots[currentSpot].position;
                Flip();
            }
            else
            {
                Debug.Log("Entrou no patrolling   5");
                myrBody.velocity = spotDir.normalized * speed;

                Debug.Log("velocity " + myrBody.velocity);
            }
        }

        /// <summary>
        /// This method changes the state if the player was found
        /// </summary>
        private void FindPlayer()
        {
            Debug.Log("Entrou no Find Player");
            if (CheckPlayer() == true && RaycastGroundCheck() == true)
            {
                Debug.Log("Entrou no if do true");
                animator.SetBool("isChasing", true);
                animator.SetBool("isPatrolling", false);
                fsm.Transition<Chase>();
            }
        }

       
        /// <summary>
        /// Checks if the robot is walking in the ground
        /// </summary>
        private RaycastHit2D RaycastGroundCheck()
        {
            Debug.DrawRay(transform.position, Vector2.down, Color.red);
            RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, rayGroundMaxDist, LayerMask.GetMask("Ground"));

            return groundInfo;
        }

        /// <summary>
        /// This method flips the robot, according to the direction which it's heading and the Sprite flipping
        /// </summary>
        private void Flip()
        {

            if (nextPos.x < robotPos.x && sRenderer.flipX == false)
            {
                sRenderer.flipX = true;

            }
            else if (nextPos.x > robotPos.x && sRenderer.flipX == true)
            {
                sRenderer.flipX = false;
            }

        }

        /// <summary>
        /// This method checks if the player is in the chasing area, which is the space between the two chaseSpots.
        /// </summary>
        /// <returns></returns>
        private bool CheckPlayer()
        {
            Debug.Log("Entrou no CheckPlayer");
            Collider2D hits = Physics2D.OverlapArea(chaseSpots[0].position, chaseSpots[1].position, LayerMask.GetMask("Player"));
            Debug.DrawLine(chaseSpots[0].position, chaseSpots[1].position, Color.blue);
            if (hits)
            {
                Debug.Log("Entrou no if do hits");
                if (hits.CompareTag("Player"))
                {
                    Debug.Log("Entrou no if do Player");
                    return true;
                }
            }
            return false;

        }

        public override void Exit() { }

    }

}