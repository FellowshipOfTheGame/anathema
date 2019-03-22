using System;
using UnityEngine;
using Anathema.Graphics;
using Anathema.Player;
using Anathema.Saving;
using Anathema.SceneLoading;

namespace Anathema.Rooms
{
    public class BreakableDoor : MonoBehaviour
    {
        private Health health;
        private SpriteBurn spriteBurn;
        
        [SerializeField] private UniqueID uniqueID;
        /// <summary>
        /// UniqueID of the destination component.
        /// Can be an pure UniqueComponent, a Door or something else.
        /// </summary>
        [SerializeField] private UniqueID destination;

        /// <summary>
        /// The name of the loading screen scene.
        /// </summary>
        [SerializeField] private string loadingSceneName;
        /// <summary>
        /// Prevents multiple scene loads from being started.
        /// </summary>
        private bool loadStarted = false;
    
        protected virtual void Awake()
        {
            spriteBurn = GetComponent<SpriteBurn>();
            health = GetComponent<Health>();

            if (!health) Debug.LogWarning($"{gameObject.name}: BreakableDoor requires a Health component.");
            else
            {
                if (spriteBurn)
                {
                    health.OnDeath += Burn;
                    spriteBurn.OnBurnComplete += Die;
                }
                else
                {
                    health.OnDeath += Die;
                }
            }

            SceneLoader.OnSceneLoaded += DestroyIfSpawnLocation;
        }

        private void DestroyIfSpawnLocation(UniqueID destination, GameData gameData)
        {
            uniqueID.SceneName = gameObject.scene.name;
            if (destination.Equals(uniqueID))
            {
                if (spriteBurn)
                {
                    health.OnDeath -= Burn;
                }

                gameObject.SetActive(false);
            }
        }
        private void Burn()
        {
            health.OnDeath -= Burn;

            spriteBurn.Burn();
        }
        private void Die()
        {
            Load();
        }

        private void Load()
        {
            if (!loadStarted)
            {
                GameObject player = PlayerFinder.Find("Player");
                if (player)
                {
                    SceneLoader loader = new SceneLoader(loadingSceneName);

                    loader.ScenesToUnload.Add(gameObject.scene.name);
                    loader.Destination = destination;
                    var upgrades = player.GetComponent<PlayerUpgrades>();
                    if (upgrades)
                    {
                        loader.GameData = upgrades.GetDataForSaving();
                    }
                    else
                    {
                        Debug.LogWarning("Player should have PlayerUpgrades component");
                    }

                    loadStarted = true;
                    try
                    {
                        loader.FadeScenes();
                    }
                    catch (InvalidOperationException e)
                    {
                        loadStarted = false;
                    }
                }
                else
                {
                   Debug.LogError($"{gameObject.name}: BreakableDoor can't find player"); 
                }
            }
        }

        protected virtual void OnDestroy()
        {
            SceneLoader.OnSceneLoaded -= DestroyIfSpawnLocation;
            
            if (spriteBurn)
            {
                spriteBurn.OnBurnComplete -= Die;
            }
            else
            {
                health.OnDeath -= Die;
            }
        }
    }
}