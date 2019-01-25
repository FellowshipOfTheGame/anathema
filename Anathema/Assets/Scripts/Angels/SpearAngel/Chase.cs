using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Chase: Anathema.Fsm.SpearAngelState {
        [SerializeField] float maxAttackDistance;
		public override void Enter() { 
            ChasePlayer();
        }

        void ChasePlayer(){
            // a* no koto
        }

        void Update () {
            base.Update();
            if (DistanceFrom(player) > lookRadius || DistanceFrom(originLocation) > baseAreaRadius) {
                fsm.Transition<Patrol>();
            } else if (DistanceFrom(player) < maxAttackDistance) {
                fsm.Transition<Attack>();
            } else {
                ChasePlayer();
            }
        }

		public override void Exit() { }
	}
}