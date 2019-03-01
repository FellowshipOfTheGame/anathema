using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ChasingRobot
{
    public class CIdle : Anathema.Fsm.CleaningRobotState
    {
        public override void Enter() { }

        /// <summary>
        /// In this class the Update is used to check if the player can be found, and if so will change the state.
        /// </summary>
        void Update()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
             if (player != null)
            {
                animator.SetBool("isPatrolling", true);
                fsm.Transition<Patrol>();
            }
        }

        public override void Exit() { }
    }
}