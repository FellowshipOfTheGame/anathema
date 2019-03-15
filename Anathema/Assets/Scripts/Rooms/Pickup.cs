using UnityEngine;
using Anathema.Graphics;
using Anathema.Player;

namespace Anathema.Rooms
{
    public abstract class Pickup : MonoBehaviour
    {
        private Animator animator;
        protected PlayerUpgrades playerUpgrades;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerUpgrades = other.GetComponent<PlayerUpgrades>();
            }
            animator.Play("Explode");
        }
        private void Die()
        {
            HandlePickup();
            Destroy(gameObject);
        }
        protected abstract void HandlePickup();
    }
}