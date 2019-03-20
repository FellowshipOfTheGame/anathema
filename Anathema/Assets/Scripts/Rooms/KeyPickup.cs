using UnityEngine;

namespace Anathema.Rooms
{
    public class KeyPickup : Pickup
    {
        public UniqueID key;
        protected override void HandlePickup()
        {
            playerUpgrades.Keys.Add(key);
        }
    }
}