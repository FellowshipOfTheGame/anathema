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
<<<<<<< HEAD
        private SpriteBurn spriteBurn;
        private SpriteRenderer sRenderer;
        private Patrol dronePatrol;
        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteBurn = GetComponent<SpriteBurn>();
            sRenderer = GetComponent<SpriteRenderer>();
            dronePatrol = GetComponent<Patrol>();
=======
        public bool damage;
        void Awake()
        {
            animator = GetComponent<Animator>();
>>>>>>> origin/demoura
            OnKnockback += OnHit;
        }

        void OnHit(Vector2 hitVector)
        {
            if (hitVector.x >= 0 && sRenderer.flipX == true)
            {
                dronePatrol.horizontalSpeed = -dronePatrol.horizontalSpeed;
                sRenderer.flipX = !sRenderer.flipX;
            }
            else
            {
                if (sRenderer.flipX == false)
                {
                    dronePatrol.horizontalSpeed = -dronePatrol.horizontalSpeed;
                    sRenderer.flipX = !sRenderer.flipX;
                }
            }
            animator.Play("DamageFeedback");
        }
    }
}
