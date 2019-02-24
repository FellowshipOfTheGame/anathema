using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.SpearAngel {
	public class Chase: Anathema.Fsm.SpearAngelState {
        [SerializeField] float maxAttackDistance;
		[SerializeField] private float UpdateRate;
		private Seeker seeker;
		private Path path;
		private Vector3 direction;
		private int currentWayPoint = 0;
		private float nextWayPointDistance = 1;

		/// <summary>
		/// Search for a path to chase the player
		/// </summary>
		public override void Enter() { 
			seeker =  GetComponent<Seeker>();
            InvokeRepeating("UpdatePath", 0f, 1/UpdateRate);
        }

        /// <summary>
        /// Checks if the angel can see the player, and if it's on its base area. If so, start patrolling.
        /// Then checks if it'ss near enough to attack.
        /// </summary>
        new void Update () {
            base.Update();
            if (DistanceFrom(player) > lookRadius || DistanceFrom(originLocation) > baseAreaRadius) {
				animator.SetBool("isFlying", true);
				animator.SetBool("isAttacking", false);
                fsm.Transition<Patrol>();
            } else if (DistanceFrom(player) < maxAttackDistance) {
                fsm.Transition<Attack>();
            }
        }

		/// <summary>
		/// Moves the angel according to path
		/// </summary>
		void FixedUpdate() {
			if (path != null) {
				direction = (path.vectorPath[currentWayPoint] - this.transform.position).normalized * speed;
				rBody.velocity = direction;

				if (DistanceFrom(path.vectorPath[currentWayPoint]) < nextWayPointDistance) {
					currentWayPoint++;
				}			
			}
		}

		/// <summary>
		/// Recalculates the path
		/// </summary>
		private void UpdatePath() {
			seeker.StartPath(this.transform.position, player.transform.position, OnPathComplete);
		}


		/// <summary>
		/// Sets the current way point to the first point of path, and saves the path on a variable
		/// </summary>
		/// <param name="p">Path gotten from A* Pathfinding</param>
		public void OnPathComplete(Path p) {
			currentWayPoint = 0;
			path = p;
		}

		public override void Exit() { }
	}
}