using UnityEngine;
using Anathema.Graphics;
using Anathema.Player;

namespace Anathema.Rooms
{
    public abstract class Pickup : MonoBehaviour
    {
        private SpriteBurn spriteBurn;
        protected PlayerUpgrades playerUpgrades;
        private void Start()
        {
            spriteBurn = GetComponent<SpriteBurn>();
            spriteBurn.OnBurnComplete += Die;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerUpgrades = other.GetComponent<PlayerUpgrades>();
                spriteBurn.Burn();
            }
        }
        private void Die()
        {
            spriteBurn.OnBurnComplete -= Die;
            HandlePickup();
            Destroy(gameObject);
        }
        protected abstract void HandlePickup();
    }
}