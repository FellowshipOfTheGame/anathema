using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.SpearAngel {
	public class Chase: Anathema.Fsm.SpearAngelState {
        [SerializeField] float maxAttackDistance;
		private Seeker seeker;

		public override void Enter() { 
			seeker =  GetComponent<Seeker>();
            ChasePlayer();
        }

        void ChasePlayer() {
            seeker.StartPath(this.transform.position, player.transform.position);
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