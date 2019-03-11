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
        private KnockBack knockBack;
        private SpriteRenderer sRenderer;
        void Awake()
        {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            spriteBurn = GetComponent<SpriteBurn>();
            knockBack = GetComponent<KnockBack>();
            sRenderer = GetComponent<SpriteRenderer>();
            OnKnockback += OnHit;
            OnDeath += DeathAnimation;
            spriteBurn.OnBurnComplete += Die;
        }

        void OnHit(Vector2 hitVector)
        {
            if (hitVector.x >= 0)
            {
                knockBack.knockbackDir = Vector2.right;
            }
            else
            {
                knockBack.knockbackPwr = -knockBack.knockbackPwr;
                knockBack.knockbackDir = Vector2.left;
            }
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
