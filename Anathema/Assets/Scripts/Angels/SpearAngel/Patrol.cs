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

			if (DistanceFrom(originLocation) > baseAreaRadius) {
				//a* no koto
			} else if (CanSeePlayer()) {
				fsm.Transition<Chase>();
			} else {
				Patrolling();
			}
		}

		public override void Exit() { }
	}
}