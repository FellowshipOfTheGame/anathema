using UnityEngine;
using UnityEngine.Tilemaps;

namespace Anathema.Rooms
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraBoundsSwitcher : MonoBehaviour
    {
        [SerializeField] private bool useCustomBounds = false;
        
        [HideInInspectorIfNot(nameof(useCustomBounds))]
        [SerializeField] private CompositeCollider2D customBounds;
        
        private ConfinerFinder confinerFinder;
        private CompositeCollider2D compositeCollider2D;

        private void Awake()
        {
            confinerFinder = GetComponentInParent<ConfinerFinder>();
    
            if (!useCustomBounds)
            {
                compositeCollider2D = GetComponent<CompositeCollider2D>();
            } 
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
           if (other.CompareTag("Player"))
            {
                if (useCustomBounds)
                {
                    confinerFinder.ConfinerBounds = customBounds;
                }
                else
                {
                    confinerFinder.ConfinerBounds = compositeCollider2D;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Vector2 viewportPoint = Camera.main.WorldToViewportPoint(other.transform.position);
                viewportPoint -= new Vector2(0.5f, 0.5f);
                if (Mathf.Abs(viewportPoint.x) > 0.5f || Mathf.Abs(viewportPoint.y) > 0.5f)
                {
                    if (useCustomBounds)
                    {
                        confinerFinder.ConfinerBounds = customBounds;
                    }
                    else
                    {
                        confinerFinder.ConfinerBounds = compositeCollider2D;
                    }
                }
            }
        }
    }
}