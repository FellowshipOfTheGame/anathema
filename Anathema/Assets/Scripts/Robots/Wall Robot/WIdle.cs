using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIdle : Anathema.Fsm.WallRobotState
{
		public override void Enter() { }

		/// <summary>
		/// In this class, Update is used to check if there is player and switch the state if so.
		/// </summary>
		void Update()
		{
			
			if((GameObject.FindGameObjectWithTag("Player")) != null)
			{
				
				animator.SetBool("isWalking", true);
				
				if(transform.localScale.x > 0)
				{
					fsm.Transition<Right>();	
				}
				else if(transform.localScale.x < 0)
				{
					fsm.Transition<Left>();
				}
			} 
		}

	public override void Exit() { }
}