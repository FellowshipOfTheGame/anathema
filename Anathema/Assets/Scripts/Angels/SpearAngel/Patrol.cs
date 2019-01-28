﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Anathema.SpearAngel {
	public class Patrol : Anathema.Fsm.SpearAngelState {
		[SerializeField] protected LayerMask enemyLookLayer	;
		[SerializeField] private float patrollingDistance;
		[SerializeField] private float UpdateRate;
		private Vector2[] patrollingPoints = new Vector2[8];
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
			SetPatrollingPoints();

			InvokeRepeating("UpdatePath", 0f, 1f/UpdateRate);
		}

		/// <summary>
		/// Checks if the angel is out of its patrol area, moving it back if so.
		/// If it can see the player, changes its state to chase the player. Otherwise, just keeps patrolling.
		/// </summary>
		void Update() {
			base.Update();

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
		}

		/// <summary>
		/// Moves the angel according to path
		/// </summary>
		void FixedUpdate() {
			if (path != null) {
				direction = (path.vectorPath[currentWayPoint] - this.transform.position).normalized * speed;
				rBody.velocity = direction;

				if (DistanceFrom(patrollingPoints[position]) < 1) {
					Debug.Log("Reached destination point");
					moving = false;
				}

				if (DistanceFrom(path.vectorPath[currentWayPoint]) < nextWayPointDistance) {
					currentWayPoint++;
				}			
			}
		}

		/// <summary>
		/// Sets 8 positions around angel's origin location, to where angel will walk while patrolling
		/// </summary>
		void SetPatrollingPoints() {
			patrollingPoints[0] = new Vector2(originLocation.x, originLocation.y + patrollingDistance);
			patrollingPoints[1] = new Vector2(originLocation.x + patrollingDistance, originLocation.y + patrollingDistance);
			patrollingPoints[2] = new Vector2(originLocation.x + patrollingDistance, originLocation.y);
			patrollingPoints[3] = new Vector2(originLocation.x + patrollingDistance, originLocation.y - patrollingDistance);
			patrollingPoints[4] = new Vector2(originLocation.x, originLocation.y - patrollingDistance);
			patrollingPoints[5] = new Vector2(originLocation.x - patrollingDistance, originLocation.y - patrollingDistance);
			patrollingPoints[6] = new Vector2(originLocation.x - patrollingDistance, originLocation.y);
			patrollingPoints[7] = new Vector2(originLocation.x - patrollingDistance, originLocation.y + patrollingDistance);
		}

		/// <summary>
		/// Sets randomly a new position to patrol, and sets it as angel's new destination
		/// </summary>
		private void Patrolling() {
			moving = true;
			Debug.Log("Patrolling");
			int newPosition;
			do {
				newPosition = Random.Range(0, 7);
			} while (newPosition == position);

			position = newPosition;
			destination = patrollingPoints[newPosition];
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
				Debug.Log("Player is too distant");
				return false;
			} else if (!LookingToPlayer()) {
				return false;
			} else if (!TryRaycasts()) {
				return false;
			} else {
				Debug.Log("Hehehe, I found you!");
				return true;
			}
		}

		/// <summary>
		/// Checks if Angel is looking to player's direction
		/// </summary>
		/// <returns>Returns true if angel is looking to player's direction</returns>
		protected bool LookingToPlayer() {
			if ((HorizontalDistanceFromPlayer() > 0 && lookingRight) || (HorizontalDistanceFromPlayer() < 0 && !lookingRight)) {
				Debug.Log("LookingToPlayer");
				return true;
			} else {
				Debug.Log("NotLookingToPlayer");
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