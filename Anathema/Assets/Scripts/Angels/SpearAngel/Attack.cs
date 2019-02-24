using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Attack : Anathema.Fsm.SpearAngelState {
		[SerializeField] float cooldown;
		[SerializeField] float attackDuration;
		public bool attacking;
		private Vector2 attackDirection;

		/// <summary>
		/// Attacks player and start cooldown timer
		/// </summary>
		public override void Enter() { 
			StartCoroutine("AttackPlayer");
			StartCoroutine("AttackCooldown");
		}

		/// <summary>
		/// Makes the angel dive in player's direction, attacking it
		/// </summary>
		/// <returns></returns>
		private IEnumerator AttackPlayer() { 
			attacking = true;
			attackDirection = player.transform.position - this.transform.position;
			rBody.velocity = attackDirection.normalized * speed;
			yield return new WaitForSeconds(attackDuration);
			attacking = false;
			rBody.velocity = Vector2.zero;
		}

		/// <summary>
		/// Waits for cooldown time to enable player to attack again
		/// </summary>
		/// <returns></returns>
		public IEnumerator AttackCooldown() {
			yield return new WaitForSeconds(cooldown);
			fsm.Transition<Chase>();
		}

		/// <summary>
		///	Stops the attack if it hits something 
		/// </summary>
		void OnCollisionEnter2D() {
			attacking = false;
			rBody.velocity = Vector2.zero;
		}

		new void Update() {
			base.Update();
		}
		public override void Exit() { }
	}
}