using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Anathema.Graphics;

namespace Anathema.ChasingRobot
{
    public class CleaningHealth : Health
    {
        private Animator animator;
        private Anathema.Fsm.FiniteStateMachine fsm;
        private SpriteBurn spriteBurn;
        void Awake()
        {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            spriteBurn = GetComponent<SpriteBurn>();
            OnKnockback += OnHit;
            OnDeath += DeathAnimation;
            spriteBurn.OnBurnComplete += Die;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
            fsm.Transition<KnockBack>();
        }

        void DeathAnimation()
        {
            spriteBurn.Burn();
            OnDeath -= DeathAnimation;
        }

        void Die()
        {
            OnKnockback -= OnHit;
            spriteBurn.OnBurnComplete -= Die;
            Destroy(this.gameObject);
        }
    }
}
