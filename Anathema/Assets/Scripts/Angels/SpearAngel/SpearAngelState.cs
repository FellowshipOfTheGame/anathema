using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Fsm {
	public abstract class SpearAngelState : FsmState {
		protected Rigidbody2D rBody;
		protected Vector3 originLocation;
		[SerializeField] protected float lookRadius;
		[SerializeField] protected float baseAreaRadius;
		[SerializeField] protected bool lookingRight = true;
		[SerializeField] protected GameObject player;


		new void Awake() {
			base.Awake();
			rBody = GetComponent<Rigidbody2D>();
			player = GameObject.Find("Player");
			originLocation = this.transform.position;
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
		/// Draws angel's look radius, that represents its sight area
		/// </summary>
		void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, lookRadius);
		}
	}
}