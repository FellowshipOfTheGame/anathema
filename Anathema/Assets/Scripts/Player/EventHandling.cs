using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Anathema.Player
{
    public class EventHandling : MonoBehaviour
    {
        private Health playerHealth;
        private Damage playerDamageState;
        private Anathema.Fsm.FiniteStateMachine fsm;
        private SpriteRenderer sRenderer;

        private void Start()
        {
            playerHealth = GetComponent<Health>();
            playerDamageState = GetComponent<Damage>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            sRenderer = GetComponent<SpriteRenderer>();

            playerHealth.OnKnockback += HandleKnockback;

            //playerHealth.Damage(1, Vector2.right, Health.DamageType.EnemyAttack);
        }

        public void HandleKnockback(Vector2 hitVector)
        {
            if(hitVector.x >= 0)
            {
                sRenderer.flipX = true;
                playerDamageState.knockbackDirection = Vector2.right;
            }
            else
            {
                sRenderer.flipX = false;
                playerDamageState.knockbackDirection = Vector2.left;
            }

            fsm.Transition<Damage>();    
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.O))
                playerHealth.Damage(2, Vector2.right, Health.DamageType.EnemyAttack);
            if(Input.GetKeyDown(KeyCode.I))
                playerHealth.Damage(2, Vector2.left, Health.DamageType.EnemyAttack);
        }
    }
}