using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.ArcherAngel {
	public class Patrol : Anathema.Fsm.ArcherAngelState {
		[SerializeField] private float patrollingDistance;
		[SerializeField] private float UpdateRate;
		[SerializeField] private GameObject[] patrollingPoints;
		private Seeker seeker;
		private bool moving;
		private int position;
		private Vector3 destination;
		private Path path;
		private Vector3 direction;
		private int currentWayPoint = 0;
		private float nextWayPointDistance = 1;
		
		/// <summary>
		/// Set patroling points and search for a path to patrol
		/// </summary>
		public override void Enter() { 
			seeker =  GetComponent<Seeker>();

			moving = false;

			InvokeRepeating("UpdatePath", 0f, 1f/UpdateRate);
		}

		new void Update() {
			base.Update();

		}

		/// <summary>
		/// Checks if the angel is out of its patrol area, moving it back if so.
		/// If it can see the player, changes its state to chase the player. Otherwise, just keeps patrolling
		/// (moving the angel according to path)
		/// </summary>
		void FixedUpdate() {
			if (DistanceFrom(originLocation) > baseAreaRadius) {
				Debug.Log("ReturningToBase");
				destination = originLocation;
			} else if (CanSeePlayer()) {
				CancelInvoke();
				rBody.velocity = Vector2.zero;
				fsm.Transition<Attack>();
			} else if (!moving) {
				Patrolling();
			}

			if (path != null) {
				direction = (path.vectorPath[currentWayPoint] - this.transform.position).normalized * speed;
				rBody.velocity = direction;

				if (DistanceFrom(destination) < 1) {
					moving = false;
					rBody.velocity = Vector2.zero;					
				}

				if (DistanceFrom(path.vectorPath[currentWayPoint]) < nextWayPointDistance) {
					currentWayPoint++;
				}			
			}
		}


		/// <summary>
		/// Sets randomly a new position to patrol, and sets it as angel's new destination
		/// </summary>
		private void Patrolling() {
			moving = true;
			int newPosition;
			do {
				newPosition = Random.Range(0, patrollingPoints.Length - 1);
			} while (newPosition == position);

			position = newPosition;
			destination = patrollingPoints[newPosition].transform.position;
		}

		/// <summary>
		/// Sets the current way point to the first point of path, and saves the path on a variable
		/// </summary>
		/// <param name="p">Path gotten from A* Pathfinding</param>
		public void OnPathComplete(Path p) {
			currentWayPoint = 0;
			path = p;
		}

		/// <summary>
		/// Recalculates the path
		/// </summary>
		private void UpdatePath() {
			seeker.StartPath(this.transform.position, destination, OnPathComplete);
		}
		public override void Exit() { }
	}
}