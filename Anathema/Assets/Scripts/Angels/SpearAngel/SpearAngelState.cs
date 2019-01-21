using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm {
	public abstract class SpearAngelState : FsmState {
		protected Rigidbody2D rBody;
		protected Vector3 originLocation;
		[SerializeField] protected float lookRadius;
		[SerializeField] protected float baseAreaRadius;
		protected bool lookingRight;
		[SerializeField] protected float playerDistanceOffset;
		[SerializeField] protected GameObject player;


		new void Awake() {
			base.Awake();
			rBody = GetComponent<Rigidbody2D>();
			player = GameObject.Find("Player");
			originLocation = this.transform.position;
		}

		protected float DistanceFromPlayer() {
			return Vector2.Distance(player.transform.position, this.transform.position);
		}


		protected bool LookingToPlayer() {
			if (DistanceFromPlayer() < playerDistanceOffset) {
				return true;
			} else if ((DistanceFromPlayer() > 0 && lookingRight) || (DistanceFromPlayer() < 0 && !lookingRight)) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// Uses raycasts to check if there is something blocking the enemy vision of the player
		/// </summary>
		/// <returns>Returns true if any of the raycasts hit the player</returns>
		protected bool TryRaycasts() {
			RaycastHit2D hit = new RaycastHit2D();
			hit = Physics2D.Raycast(this.transform.position, player.transform.position);
			Debug.DrawLine(this.transform.position, player.transform.position, Color.green);
			Debug.Log(hit);
			return false;
		}

		/// <summary>
		/// Checks if enemy can see the player
		/// </summary>
		/// <returns>Returns true or false whether enemy can see player or not</returns>
		protected bool CanSeePlayer() {
			if (DistanceFromPlayer() > lookRadius) {
				return false;
			} else if (!LookingToPlayer()) {
				return false;
			} 

			if (DistanceFromPlayer() < lookRadius && LookingToPlayer()) {
				TryRaycasts();
			}

			return true;
		}

		protected void CheckSide() {
			if (rBody.velocity.x > 0f) {
				lookingRight = true;
			} else if (rBody.velocity.x < 0) {
				lookingRight = false;
			}
		}

		void OnDrawGizmosSelectedm () {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, lookRadius);
		}
	}
}