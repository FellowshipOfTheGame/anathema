using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CIdle : Anathema.Fsm.CleaningRobotState
{
	/// <summary>
	/// In this class, the Enter is used check if the player can be found and switches the movement state 
	/// </summary>
	public override void Enter()
	{
		if((GameObject.FindGameObjectWithTag("Player")) != null)
		{
			animator.SetBool("isPatrolling", true);
			fsm.Transition<Patrol>();
		} 

	}

	public override void Exit() { }
}