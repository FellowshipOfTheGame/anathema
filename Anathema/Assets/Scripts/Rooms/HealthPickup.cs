using UnityEngine;

namespace Anathema.Rooms
{
    public class HealthPickup : Pickup
    {
        [SerializeField] private int healthBonus = 10;
        protected override void HandlePickup()
        {
            playerUpgrades.MaxHealth += healthBonus;
        }
    }
}