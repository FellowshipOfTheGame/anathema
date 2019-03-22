using UnityEngine;
using Anathema.Graphics;
using Anathema.Player;

namespace Anathema.Rooms
{
    public abstract class Pickup : UniqueComponent
    {
        [SerializeField] private bool shouldExplode = true;
        [SerializeField] private string playerSceneName = "Player";
        private Animator animator;
        protected PlayerUpgrades playerUpgrades;

        protected override void Awake()
        {
            base.Awake();
            if (shouldExplode)
                animator = GetComponent<Animator>();

            var player = PlayerFinder.Find(playerSceneName);
            playerUpgrades = player.GetComponent<PlayerUpgrades>();

            if (playerUpgrades)
            {
                if (playerUpgrades.DiscoveredPickups.Contains(UniqueID))
                {
                    Destroy(this.gameObject);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (shouldExplode)
                {
                    animator.Play("Explode");
                }
                else
                {
                    Die();
                }
            }
        }
        private void Die()
        {
            HandlePickup();
            playerUpgrades.DiscoveredPickups.Add(UniqueID);
            Destroy(gameObject);
        }
        protected abstract void HandlePickup();
    }
}