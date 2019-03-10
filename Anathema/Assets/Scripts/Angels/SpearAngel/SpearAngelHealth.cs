using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anathema.Graphics;

namespace Anathema.SpearAngel {
	public class SpearAngelHealth : Health {
        private Animator animator;
        private SpriteBurn spriteBurn;
        private Anathema.Fsm.FiniteStateMachine fsm;

        void Awake() {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            spriteBurn = GetComponent<SpriteBurn>();
            OnKnockback += OnHit;
            OnDeath += DeathAnimation;
            spriteBurn.OnBurnComplete += Die;
        }

        void OnHit(Vector2 hitVector) {
            // Debug.Log("knockBack");
            // animator.Play("DamageFeedback");
        }
        
        void DeathAnimation()
        {
            OnDeath -= DeathAnimation;
            spriteBurn.Burn();
        }

        void Die() {
            OnKnockback -= OnHit;
            spriteBurn.OnBurnComplete -= Die;
            Destroy(this.gameObject);
        }
    }
}
