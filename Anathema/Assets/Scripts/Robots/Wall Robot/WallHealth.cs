using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Anathema.Graphics;

namespace Anathema.WallRobot
{
    public class WallHealth : Health
    {
        private Animator animator;
        private SpriteBurn spriteBurn;
        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteBurn = GetComponent<SpriteBurn>();
            OnKnockback += OnHit;
            OnDeath += DeathAnimation;
            spriteBurn.OnBurnComplete += Die;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
        }
        void DeathAnimation()
        {
            OnDeath -= DeathAnimation;
            spriteBurn.Burn();
        }
        void Die()
        {
            OnKnockback -= OnHit;
            spriteBurn.OnBurnComplete -= Die;
            Destroy(this.gameObject);
        }
    }
}
