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
        [SerializeField] private string loadingScene;
        [SerializeField] private string playerScene;
        private Health playerHealth;
        private Damage playerDamageState;
        private PlayerUpgrades playerUpgrades;
        private SpriteBurn playerBurn;
        private Anathema.Fsm.FiniteStateMachine fsm;
        private SpriteRenderer sRenderer;

        private void Start()
        {
            playerHealth = GetComponent<Health>();
            playerDamageState = GetComponent<Damage>();
            playerUpgrades = GetComponent<PlayerUpgrades>();
            playerBurn = GetComponent<SpriteBurn>();
            fsm = GetComponent<Anathema.Fsm.FiniteStateMachine>();
            sRenderer = GetComponent<SpriteRenderer>();

            playerHealth.OnKnockback += HandleKnockback;
            playerHealth.OnDeath += HandleDeath;

            playerBurn.OnBurnComplete += ReloadSave;

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
        public void HandleDeath()
        {
            playerBurn.Burn();
            playerHealth.OnDeath -= HandleDeath;
            playerHealth.OnKnockback -= HandleKnockback;
        }
        private void ReloadSave()
        {
            SaveProfile saveProfile = new SaveProfile(playerUpgrades.ProfileName);
            GameData gameData = saveProfile.Load();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                
                if (scene.name != playerScene)
                {
                    SceneLoader loader = new SceneLoader(loadingScene);
                    loader.FadeScenes(scene.name, playerScene, gameData, reloadPlayerScene: true);
                    break;
                }
            }
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.O))
                playerHealth.Damage(2, Vector2.right, Health.DamageType.EnemyAttack);
            if(Input.GetKeyDown(KeyCode.I))
                playerHealth.Damage(2, Vector2.left, Health.DamageType.EnemyAttack);
        }
    }
}