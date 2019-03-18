using Anathema.Graphics;
using UnityEngine;

namespace Anathema.Rooms
{
    public class MeltableDoor : MonoBehaviour
    {
        [SerializeField] private Vector3 fireScale;
        [SerializeField] private Vector2 fireOffset;
        [SerializeField] private float firePlaybackSpeed;
        [SerializeField] private bool destroyed;
        private SpriteBurn spriteBurn;
        private Collider2D mainCollider2D;
        private void Start()
        {
            spriteBurn = GetComponent<SpriteBurn>();
            mainCollider2D = GetComponent<Collider2D>();
            
            spriteBurn.OnBurnComplete += DisableCollision;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!destroyed)
            {
                Fire fire = other.GetComponent<Fire>();

                if (fire)
                {
                    Animator fireAnimator = other.GetComponent<Animator>();
                    destroyed = true;
                    fire.transform.localScale = fireScale;
                    fire.transform.localPosition += (Vector3) fireOffset;

                    fireAnimator.speed = firePlaybackSpeed;

                    if (fire)
                    {
                        spriteBurn.Burn();
                    }
                }

            }

        }

        private void DisableCollision()
        {
            mainCollider2D.enabled = false;
        }

        private void OnDestroy()
        {
            spriteBurn.OnBurnComplete -= DisableCollision;
        }
    }
}