using UnityEngine;

namespace Anathema.Rooms
{
    public class KeyPickup : Pickup
    {
        [SerializeField] private UniqueID key;
        protected override void HandlePickup()
        {
            playerUpgrades.Keys.Add(key);
        }
    }
}