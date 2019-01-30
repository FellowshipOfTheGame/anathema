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
    public class Door : UniqueComponent
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
        /// For ignoring the first trigger enter when player spawns on trigger
        /// </summary>
        public bool IgnoreNextCollision { get; set; }
        /// <summary>
        /// Prevents multiple scene loads from being started.
        /// </summary>
        private bool loadStarted = false;
        protected override void Awake()
        {
            base.Awake();

            Collider2D myCollider2D = GetComponent<Collider2D>();
            if (!myCollider2D) Debug.LogWarning($"{gameObject.name}: Door requires a Collider2D component.");
            else if (!myCollider2D.isTrigger) Debug.LogWarning($"{gameObject.name}: Door requires that the Collider2D is a trigger.");
        }
        private void OnTriggerEnter2D(Collider2D collider)
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
                    loader.FadeScenes(gameObject.scene.name, destination, collider.gameObject);
                }
            }
        }
    }
}