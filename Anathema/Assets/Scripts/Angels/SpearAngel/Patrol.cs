using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.SpearAngel {
	public class Patrol : Anathema.Fsm.SpearAngelState {
		[SerializeField] protected LayerMask enemyLookLayer	;
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
				fsm.Transition<Chase>();
			} else if (!moving) {
				Patrolling();
			}

			if (path != null) {
				direction = (path.vectorPath[currentWayPoint] - this.transform.position).normalized * speed;
				rBody.velocity = direction;

				if (DistanceFrom(patrollingPoints[position]) < 1) {
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

		/// <summary>
		/// Calculates angel's horizontal distance from player. 
		/// Positive distances mean that the player is on the right, and negative mean the player is on the left.
		/// </summary>
		/// <returns>Retunrs the distance value, positive or negative depending on where the player is</returns>
		protected float HorizontalDistanceFromPlayer() {
			return player.transform.position.x - this.transform.position.x;
		}

		/// <summary>
		/// /// Checks if enemy can see the player	
		/// </summary>
		/// <returns>Returns true or false whether enemy can see player or not</returns>
		protected bool CanSeePlayer() {
			if (DistanceFrom(player) > lookRadius) {
				return false;
			} else if (!LookingToPlayer()) {
				return false;
			} else if (!TryRaycasts()) {
				return false;
			} else {
				Debug.LogWarning("Hehehe, I found you!");
				return true;
			}
		}

		/// <summary>
		/// Checks if Angel is looking to player's direction
		/// </summary>
		/// <returns>Returns true if angel is looking to player's direction</returns>
		protected bool LookingToPlayer() {
			if ((HorizontalDistanceFromPlayer() > 0 && lookingRight) || (HorizontalDistanceFromPlayer() < 0 && !lookingRight)) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Uses raycast(s) to check if there is something blocking the enemy vision of the player
		/// </summary>
		/// <returns>Returns true if (any of the) raycast(s) hit the player</returns>
		protected bool TryRaycasts() {
			RaycastHit2D hit = new RaycastHit2D();
			hit = Physics2D.Raycast(this.transform.position, player.transform.position, Mathf.Infinity, enemyLookLayer);
			Debug.DrawLine(this.transform.position, player.transform.position, Color.green);
			
			if (hit) {
				Debug.Log(hit.collider.gameObject.name);
				if (hit.collider.gameObject.name == "Player") {
					return true;
				} else {
					return false;
				}
			} else {
				Debug.Log("Didn't hit");
				return false;
			}
		}
		public override void Exit() { }
	}
}