using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;
using Anathema.Saving;
using Anathema.Graphics;

namespace Anathema.Player
{
    public class EventHandling : MonoBehaviour
    {
        private Health playerHealth;
        private Damage playerDamageState;
        private PlayerUpgrades playerUpgrades;
        private SpriteBurn playerBurn;
        private Anathema.Fsm.FiniteStateMachine fsm;
        private SpriteRenderer sRenderer;

        private void Awake()
        {
            playerHealth = GetComponent<Health>(); //This needs to be set before OnEnable
            playerDamageState = GetComponent<Damage>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            sRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            playerHealth.OnKnockback += HandleKnockback;
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

        private void OnDisable()
        {
            playerHealth.OnKnockback -= HandleKnockback;
        }

        // private void Update() {
        //     if(Input.GetKeyDown(KeyCode.O))
        //         playerHealth.Damage(2, Vector2.right, Health.DamageType.EnemyAttack);
        //     if(Input.GetKeyDown(KeyCode.I))
        //         playerHealth.Damage(2, Vector2.left, Health.DamageType.EnemyAttack);
        // }
    }
}