using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Attack : Anathema.Fsm.SpearAngelState {
		[SerializeField] float cooldown;

		public override void Enter() { 
			AttackPlayer();
			StartCoroutine("AttackCooldown");
		}

		private void AttackPlayer() { }

		/// <summary>
		/// Wait for cooldown time to enable player to attack again
		/// </summary>
		/// <returns></returns>
		public IEnumerator AttackCooldown() {
			yield return new WaitForSeconds(cooldown);
			fsm.Transition<Chase>();
		}

		public override void Exit() { }
	}
}