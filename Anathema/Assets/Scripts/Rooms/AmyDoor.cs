using Anathema.Player;
using Anathema.Rooms;
using UnityEngine;

namespace Rooms
{
    public class AmyDoor : CutsceneDoor
    {
        protected override bool LoadCutscene(GameObject player)
        {
            PlayerUpgrades upgrades = player.GetComponent<PlayerUpgrades>();

            if (upgrades.HasScythe)
            {
                return false;
            }
            else
            {
                return true;
            }
        }    }
}