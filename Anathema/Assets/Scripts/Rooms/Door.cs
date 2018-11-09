using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;

namespace Anathema.Rooms
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : UniqueComponent
    {
        [SerializeField] private UniqueID destination;

        [SerializeField] private string loadingSceneName;
        private new int collider2D;
        public bool IgnoreNextCollision { get; set;} //For ignoring the first trigger enter when player spawns on trigger
        private Collider2D myCollider2D;
        private bool loadStarted = false;
        protected override void Awake()
        {
            base.Awake();

            myCollider2D = GetComponent<Collider2D>();
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
                    // Debug.Log("You entered the trigger");
                    SceneLoader loader = new SceneLoader(loadingSceneName);
                    loader.FadeScenes(gameObject.scene.name, destination, collider.gameObject);
                }
            }
        }
    }
}