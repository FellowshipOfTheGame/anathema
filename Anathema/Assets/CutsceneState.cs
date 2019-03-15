using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	/// <summary>
	/// 	This is a void, empty state to prevent input during cutscenes.
	/// </summary>
	public class CutsceneState : Anathema.Fsm.PlayerState
	{
		public override void Enter()
		{
			
		}

		public override void Exit()
		{

		}
	}
}