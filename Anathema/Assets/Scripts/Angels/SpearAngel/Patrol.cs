using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Patrol : Anathema.Fsm.SpearAngelState {
		[SerializeField] protected LayerMask enemyLookLayer	;
		[SerializeField] float patrollingDistance;
		private Vector2[] patrollingPoints = new Vector2[8];
		private bool moving;
		private bool checkingSides;
		private int position;
		
		public override void Enter() { 
			moving = false;
			checkingSides = false;
			SetPatrollingPoints();
		}

		void Update() {
			base.Update();

			if (DistanceFrom(originLocation) > baseAreaRadius) {
				//a* no koto
			} else if (CanSeePlayer()) {
				fsm.Transition<Chase>();
			} else if (!moving && !checkingSides) {
				Patrolling();
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

		private void Patrolling() {
			int newPosition;
			do {
				newPosition = Random.Range(0, 7);
			} while (newPosition == position);

			this.transform.position = patrollingPoints[newPosition];
			position = newPosition;
			StartCoroutine("Wait");
		}

		IEnumerator Wait() {
			moving = true;
			yield return new WaitForSeconds(2);
			moving = false;
		}

		private IEnumerator CheckBothSides() {
			checkingSides = true;
			rBody.velocity = Vector2.right;
			yield return new WaitForSeconds(1);
			rBody.velocity = Vector2.left;
			yield return new WaitForSeconds(1);
			rBody.velocity = Vector2.zero;
			checkingSides = false;
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