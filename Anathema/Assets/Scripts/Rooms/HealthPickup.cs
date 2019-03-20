using Anathema.Player;
using UnityEngine;

namespace Anathema.Rooms
{
    public class HealthPickup : Pickup
    {
        [SerializeField] private int healthBonus = 10;
        protected override void HandlePickup()
        {
            playerUpgrades.MaxHealth += healthBonus;
            Health health  = playerUpgrades.GetComponent<Health>();
            if (health)
            {
                health.Heal(playerUpgrades.MaxHealth);
            }
        }
    }
}