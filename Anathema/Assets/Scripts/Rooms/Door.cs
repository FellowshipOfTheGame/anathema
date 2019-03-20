using Anathema.Player;
using Anathema.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;

namespace Anathema.Rooms
{
    /// <summary>
    /// Used for making teleport door between scenes.
    /// Requires a Trigger Collider on the same GameObject.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Door : UniqueTrigger
    {
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
                    loader.Destination = destination;
                    
                    var upgrades = GetComponent<PlayerUpgrades>();
                    if (upgrades)
                    {
                        loader.GameData = GetComponent<GameData>();
                    }
                    
                    loader.FadeScenes();
                }
            }
        }
    }
}