using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.SpearAngel {
	public class SpearAngelHealth : Health {
        private Animator animator;
        // private Anathema.Fsm.FiniteStateMachine fsm;

        void Awake() {
            animator = GetComponent<Animator>();
            OnKnockback += OnHit;
            OnDeath += Die;
        }

        void OnHit(Vector2 hitVector) {
            // Debug.Log("knockBack");
            // animator.Play("DamageFeedback");
        }

        void Die() {
            OnKnockback -= OnHit;
            OnDeath -= Die;
            Destroy(this.gameObject);
        }
    }
}
