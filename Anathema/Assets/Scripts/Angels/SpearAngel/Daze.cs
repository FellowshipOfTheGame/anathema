using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class Daze : Anathema.Fsm.SpearAngelState {
        [Tooltip("Daze time after knockback")]
        [SerializeField] private float dazeTime;

        public override void Enter() {
            animator.SetBool("isFlying", true);
            animator.SetBool("isTakingDamage", false);
            StartCoroutine(Dazed());
        }

        private IEnumerator Dazed () {
            Debug.LogWarning("Dazed");
            rBody.velocity = Vector2.zero;
            yield return new WaitForSeconds(dazeTime);
            fsm.Transition<Chase>();

        }

        public override void Exit() { }
    }
}