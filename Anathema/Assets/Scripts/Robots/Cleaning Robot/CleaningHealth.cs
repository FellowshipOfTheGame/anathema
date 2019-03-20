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
        private KnockBack knockBack;
        private SpriteRenderer sRenderer;
        void Awake()
        {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            knockBack = GetComponent<KnockBack>();
            sRenderer = GetComponent<SpriteRenderer>();
            OnKnockback += OnHit;
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
    }
}
