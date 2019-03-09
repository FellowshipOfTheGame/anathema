using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Drone
{
    public class Patrol : Anathema.Fsm.DroneState
    {
        [Tooltip("Speed in which the drone will move horizontally.")]
        [SerializeField] private float horizontalSpeed;

        [Tooltip("Speed in which the drone will move vertically.")]
        [SerializeField] private float verticalSpeed;

        [Tooltip("Amplitude in which the drone will move vertically through the room.")]
        [SerializeField] private float amplitude;

        [Tooltip("attack damage of the drone.")]
        [SerializeField] int damage;

        private DroneHealth droneHealth;

        private float time;

        //The initial position of the drone
        private Vector2 initialPos;

        private void Start()
        {
            droneHealth = GetComponent<DroneHealth>();
        }
        public override void Enter()
        {
            initialPos = transform.position;
        }

        /// <summary>
        /// In this class, the FixedUpdate is used to move the drone in a senoidal movement.
        /// </summary>
        void FixedUpdate()
        {
            myrBody.MovePosition(new Vector2(this.transform.position.x + horizontalSpeed, initialPos.y + amplitude * Mathf.Sin(verticalSpeed * time)));
            time += Time.deltaTime;

            if(droneHealth.damage == true)
            {
                horizontalSpeed = -horizontalSpeed;
                sRenderer.flipX = !sRenderer.flipX;
                droneHealth.damage = false;
            }
        }

        /// <summary>
        /// This method checks drone collisions, in general it will change the direction of the movement, and if it collides
        /// with the player, will attack.
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        void OnCollisionEnter2D(Collision2D other)
        {
            sRenderer.flipX = !sRenderer.flipX;
            if (other.collider == true)
            {
                if (other.collider.CompareTag("Player"))
                {
                    Debug.Log("Attack");
                    Vector2 hitVector = other.transform.position - transform.position;
                    other.transform.GetComponent<Health>().Damage(damage, hitVector, Health.DamageType.EnemyAttack);
                }
            }
            horizontalSpeed = -horizontalSpeed;
        }

        public override void Exit() { }
    }
}