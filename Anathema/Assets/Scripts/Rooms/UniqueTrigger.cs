using Anathema.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;

namespace Anathema.Rooms
{
    /// <summary>
    /// Base for Door and SaveStations
    /// Handles ignoring collision when spawning on top of trigger.
    /// Requires a Trigger Collider on the same GameObject.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class UniqueTrigger : UniqueComponent
    {
        /// <summary>
        /// For ignoring the first trigger enter when player spawns on trigger
        /// </summary>
        public bool IgnoreNextCollision { get; set; }

        private bool sceneFinishedLoading = false;
        protected override void Awake()
        {
            base.Awake();

            Collider2D myCollider2D = GetComponent<Collider2D>();
            if (!myCollider2D) Debug.LogWarning($"{gameObject.name}: UniqueTrigger requires a Collider2D component.");
            else if (!myCollider2D.isTrigger) Debug.LogWarning($"{gameObject.name}: UniqueTrigger  requires that the Collider2D is a trigger.");
        }

        protected virtual void OnEnable()
        {
            SceneLoader.OnSceneLoaded += SceneLoadHandler;
        }

        private void SceneLoadHandler(UniqueID destination, GameData gameData)
        {
            if (destination.Equals(this.UniqueID))
            {
                IgnoreNextCollision = true;
            }
            sceneFinishedLoading = true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (sceneFinishedLoading && collider.CompareTag("Player"))
            {
                if (IgnoreNextCollision)
                {
                    IgnoreNextCollision = false;
                }
                else
                {
                    OnTriggerActivate(collider);
                }
            }
        }
        protected abstract void OnTriggerActivate(Collider2D collider);

        protected virtual void OnDisable()
        {
            SceneLoader.OnSceneLoaded -= SceneLoadHandler;
        }
    }
}