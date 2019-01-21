using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Patrol : Anathema.Fsm.SpearAngelState {
		public override void Enter() {
			Patrolling();
		}

		private void Patrolling() {

		}

		void Update() {
			CheckSide();

			if (CanSeePlayer()) {
				Debug.Log("Mitsukatta!!");
			}
		}

		public override void Exit() {}
	}
}