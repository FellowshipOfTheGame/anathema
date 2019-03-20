using UnityEngine;

namespace Anathema.Rooms
{
    public class ScythePickup : Pickup
    {
        protected override void HandlePickup()
        {
            playerUpgrades.HasScythe = true;
        }
    }
}