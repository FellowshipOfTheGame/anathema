using UnityEngine;

namespace Anathema.Rooms
{
    public class FireAttackPickup : Pickup
    {
        protected override void HandlePickup()
        {
            playerUpgrades.HasFireAttack = true;
        }
    }
}