using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anathema.WallRobot
{
    public class WallHealth : Health
    {
        private Animator animator;
        void Awake()
        {
            animator = GetComponent<Animator>();
            OnKnockback += OnHit;
            OnDeath += Die;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
        }

        void Die()
        {
            OnKnockback -= OnHit;
            OnDeath -= Die;
            Destroy(this.gameObject);
        }
    }
}
