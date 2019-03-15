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
        void Awake()
        {
            animator = GetComponent<Animator>();
            OnKnockback += OnHit;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
        }
    }
}
