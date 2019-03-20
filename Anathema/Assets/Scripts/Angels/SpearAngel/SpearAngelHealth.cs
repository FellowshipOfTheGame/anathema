using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anathema.Graphics;

namespace Anathema.SpearAngel {
	public class SpearAngelHealth : Health {
        private Animator animator;
        private Anathema.Fsm.FiniteStateMachine fsm;

        void Awake() {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            OnKnockback += OnHit;
        }

        void OnHit(Vector2 hitVector) {
            fsm.Transition<KnockBack>();
        }
    }
}