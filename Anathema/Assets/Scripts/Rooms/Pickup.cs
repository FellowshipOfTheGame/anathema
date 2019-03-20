using UnityEngine;
using Anathema.Graphics;
using Anathema.Player;

namespace Anathema.Rooms
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] private bool shouldExplode = true;
        private Animator animator;
        protected PlayerUpgrades playerUpgrades;

        private void Start()
        {
            if (shouldExplode)
                animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerUpgrades = other.GetComponent<PlayerUpgrades>();
            }

            if (shouldExplode)
            {
                animator.Play("Explode");
            }
            else
            {
                Die();
            }
        }
        private void Die()
        {
            HandlePickup();
            Destroy(gameObject);
        }
        protected abstract void HandlePickup();
    }
}