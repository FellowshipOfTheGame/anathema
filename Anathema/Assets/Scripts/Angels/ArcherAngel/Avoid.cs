using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.ArcherAngel {
	public class Avoid : Anathema.Fsm.ArcherAngelState {
		[SerializeField] private float UpdateRate;
		[SerializeField] private GameObject[] patrollingPoints;
		private Seeker seeker;
		private Path path;
		private Vector3 direction;
		private Vector3 destination;
		private int currentWayPoint = 0;
		private float nextWayPointDistance = 1;

		/// <summary>
		/// Search for a path to chase the player
		/// </summary>
		public override void Enter() { 
			seeker =  GetComponent<Seeker>();
			destination = GetFurthesPatrollingPoint().transform.position;
            InvokeRepeating("UpdatePath", 0f, 1 / UpdateRate);
        }

        /// <summary>
        /// Checks if the angel can see the player, and if it's on its base area. If so, start patrolling.
        /// </summary>
        new void Update () {
            base.Update();
            if (DistanceFrom(originLocation) > baseAreaRadius) {
                fsm.Transition<Patrol>();
            }
        }

		/// <summary>
		/// Moves the angel according to path
		/// </summary>
		void FixedUpdate() {
			if (path != null) {
				direction = (path.vectorPath[currentWayPoint] - this.transform.position).normalized * speed;
				rBody.velocity = direction;

				if (DistanceFrom(destination) < 1) {
					rBody.velocity = Vector2.zero;					
					fsm.Transition<Attack>();
				}

				if (DistanceFrom(path.vectorPath[currentWayPoint]) < nextWayPointDistance) {
					currentWayPoint++;
				}			
			}
		}

		/// <summary>
		/// Recalculates the path
		/// </summary>
		private void UpdatePath() {
			seeker.StartPath(this.transform.position, destination, OnPathComplete);
		}

		/// <summary>
		/// Calculates distance between two objects
		/// </summary>
		/// <param name="object1">The first object</param>
		/// <param name="object2">The second object</param>
		/// <returns></returns>
		private float DistanceBetween (GameObject object1, GameObject object2) {
			return Vector2.Distance(object1.transform.position, object2.transform.position);
		}

		/// <summary>
		/// Gets the furthest patrolling point from player;
		/// </summary>
		/// <returns></returns>
		private GameObject GetFurthesPatrollingPoint() {
			GameObject furthest = patrollingPoints[0];

			foreach (GameObject point in patrollingPoints) {
				if (DistanceBetween(player, point) > DistanceBetween(player, furthest)) {
					furthest = point;
				}
			}
			return furthest;
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