using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Anathema.Graphics;

namespace Anathema.Drone
{
    public class DroneHealth : Health
    {
        private Animator animator;
        public bool damage;
        void Awake()
        {
            animator = GetComponent<Animator>();
            OnKnockback += OnHit;
        }

        void OnHit(Vector2 hitVector)
        {
            Debug.Log("knockBack");
            animator.Play("DamageFeedback");
            damage = true;
        }
    }
}
