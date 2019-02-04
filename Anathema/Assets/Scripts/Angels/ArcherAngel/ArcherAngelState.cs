using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm {
	public abstract class ArcherAngelState : FsmState {
		protected Rigidbody2D rBody;
		protected SpriteRenderer spriteRenderer;
		protected Vector3 originLocation;
		[SerializeField] protected LayerMask enemyLookLayer;
		[SerializeField] protected GameObject origin;
		[SerializeField] protected float lookRadius;
		[SerializeField] protected float baseAreaRadius;
		[SerializeField] protected bool lookingRight = true;
		[SerializeField] protected GameObject player;
		[SerializeField] protected float speed;



		new void Awake() {
			base.Awake();
			rBody = GetComponent<Rigidbody2D>();
			spriteRenderer= GetComponent<SpriteRenderer>();
			player = GameObject.Find("Player");
			originLocation = origin.transform.position;
		}

		protected void Update() {
			CheckSide();
		}

		/// <summary>
		/// Checks to which side angel is looking, and changes variables (and animations)
		/// </summary>
		protected void CheckSide() {
			if (rBody.velocity.x > 0f) {
				lookingRight = true;
			} else if (rBody.velocity.x < 0) {
				lookingRight = false;
			}
		}

		/// <summary>
		/// Calculates angel's distance from other object
		/// </summary>
		/// <returns>Returns angel's distance from other object</returns>
		protected float DistanceFrom(GameObject otherObject) {
			return Vector2.Distance(otherObject.transform.position, this.transform.position);
		}
		protected float DistanceFrom(Vector3 otherObject) {
			return Vector2.Distance(otherObject, this.transform.position);
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
				Debug.LogWarning(hit.collider.gameObject.name);
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

		/// <summary>
		/// Draws angel's look radius, that represents its sight area
		/// </summary>
		void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, lookRadius);
		}
	}
}