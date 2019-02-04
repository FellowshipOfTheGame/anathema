using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.ArcherAngel {
	public class Attack : Anathema.Fsm.ArcherAngelState {
		[SerializeField] float minAttackDistance;
		[SerializeField] float cooldown;
		[SerializeField] float attackDelay;
		[SerializeField] float arrowSpeed;
		[SerializeField] GameObject arrow;
		private Vector2 attackDirection;

		/// <summary>
		/// Attacks player and start cooldown timer
		/// </summary>
		public override void Enter() { 
			StartCoroutine("AttackPlayer");
		}

		/// <summary>
		/// Makes the angel dive in player's direction, attacking it
		/// </summary>
		/// <returns></returns>
		private IEnumerator AttackPlayer() { 
			rBody.velocity = Vector2.zero; 
			Shoot();
			yield return new WaitForSeconds(attackDelay);
			Shoot();

			StartCoroutine("AttackCooldown");
		}

		/// <summary>
		/// Shoots an arrow in player's direction
		/// </summary>
		private void Shoot() {
			GameObject arrowInstance;
			attackDirection = player.transform.position - this.transform.position;
			float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
			arrowInstance = Instantiate(arrow, this.transform.position, Quaternion.Euler(0, 0, angle));
			arrowInstance.GetComponent<Rigidbody2D>().velocity = attackDirection.normalized * arrowSpeed;
		}

		/// <summary>
		/// Waits for cooldown time and then checks if the player is too near, changing state. 
		/// If the angel can see the player, it continues shooting.
		/// </summary>
		/// <returns>Cooldown time</returns>
		public IEnumerator AttackCooldown() {
			yield return new WaitForSeconds(cooldown);

			if (DistanceFrom(player) < minAttackDistance) {
				fsm.Transition<Avoid>();
			} else if (TryRaycasts()) {
				StartCoroutine("AttackPlayer"); 
			} else {
				fsm.Transition<Patrol>();
			}
		}

		new void Update() {
			base.Update();
		}
		public override void Exit() { }
	}
}