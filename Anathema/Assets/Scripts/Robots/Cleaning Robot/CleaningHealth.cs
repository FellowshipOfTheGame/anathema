using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anathema.ChasingRobot
{
    public class CleaningHealth : Health
    {
        private Animator animator;
        private Anathema.Fsm.FiniteStateMachine fsm;
        void Awake()
        {
            animator = GetComponent<Animator>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            OnKnockback += OnHit;
            OnDeath += Die;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
            fsm.Transition<KnockBack>();
        }

        void Die()
        {
            OnKnockback -= OnHit;
            OnDeath -= Die;
            Destroy(this.gameObject);
        }
    }
}
