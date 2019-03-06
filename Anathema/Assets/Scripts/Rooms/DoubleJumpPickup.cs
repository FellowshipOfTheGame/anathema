using UnityEngine;

namespace Anathema.Rooms
{
    public class DoubleJumpPickup : Pickup
    {
        protected override void HandlePickup()
        {
            playerUpgrades.HasDoubleJump = true;
        }
    }
}