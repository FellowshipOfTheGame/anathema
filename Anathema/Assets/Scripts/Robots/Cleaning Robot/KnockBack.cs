using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anathema.ChasingRobot
{
    public class KnockBack : Anathema.Fsm.CleaningRobotState
    {
        public override void Enter()
        { 
            myrBody.AddForce(-transform.forward);
            fsm.Transition<CIdle>();
            
        }   

        public override void Exit() { }
    }
}