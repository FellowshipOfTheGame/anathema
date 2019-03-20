using Anathema.Player;
using Anathema.Saving;
using Anathema.SceneLoading;
using UnityEngine;

namespace Anathema.Rooms
{
    public abstract class CutsceneDoor : UniqueTrigger
    {
        [SerializeField] private string loadingSceneName;
        [SerializeField] protected UniqueID mainDestination;
        [SerializeField] protected UniqueID alternateDestination;
        /// <summary>
        /// Prevents multiple scene loads from being started.
        /// </summary>
        private bool loadStarted = false;

        protected abstract bool LoadCutscene(GameObject player);
        protected override void OnTriggerActivate(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                if (IgnoreNextCollision)
                {
                    IgnoreNextCollision = false;
                }
                else if (!loadStarted)
                {
                    loadStarted = true;
                    SceneLoader loader = new SceneLoader(loadingSceneName);

                    loader.ScenesToUnload.Add(gameObject.scene.name);
                    
                    if (LoadCutscene(collider.gameObject))
                    {
                        loader.Destination = alternateDestination;
                        loader.ScenesToUnload.Add(collider.gameObject.scene.name);

                        var upgrades = collider.GetComponent<PlayerUpgrades>();
                        if (upgrades)
                        {
                            loader.GameData = upgrades.GetDataForSaving();
                        }
                    }
                    else
                    {
                        loader.Destination = mainDestination;
                    }
                    
                    loader.FadeScenes();
                }
            }
        }
    }
}