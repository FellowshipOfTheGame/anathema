using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anathema.Player;

namespace Anathema.Fsm {
	public abstract class SpearAngelState : FsmState {
		protected Rigidbody2D rBody;
		protected SpriteRenderer sRenderer;
		protected Animator animator;
		protected Vector3 originLocation;
		[SerializeField] private string playerSceneName = "Player";
		[SerializeField] protected int damage;
		[SerializeField] protected GameObject origin;
		[SerializeField] protected float lookRadius;
		[SerializeField] protected float baseAreaRadius;
		[SerializeField] protected bool lookingRight = true;
		[SerializeField] protected GameObject player;
		[SerializeField] protected float speed;
		//FIXME
		// Velocity to turn angel
		[SerializeField] protected float turnVel;


		new void Awake() {
			base.Awake();
			animator = GetComponent<Animator>();
			rBody = GetComponent<Rigidbody2D>();
			sRenderer= GetComponent<SpriteRenderer>();
			player = PlayerFinder.Find(playerSceneName);
			originLocation = origin.transform.position;
		}

		protected void Update() {
			CheckSide();
		}

		/// <summary>
		/// Checks to which side angel is looking, and changes variables (and animations)
		/// </summary>
		protected void CheckSide() {
			if (rBody.velocity.x > turnVel) {
				lookingRight = true;
				sRenderer.flipX = false;
			} else if (rBody.velocity.x < -turnVel) {
				lookingRight = false;
				sRenderer.flipX = true;
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

		protected void OnCollisionEnter2D(Collision2D other) {
			if (other.gameObject.CompareTag("Player")) {
				other.gameObject.GetComponent<Health>().Damage(damage, -this.transform.position + other.transform.position, Health.DamageType.EnemyAttack);
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