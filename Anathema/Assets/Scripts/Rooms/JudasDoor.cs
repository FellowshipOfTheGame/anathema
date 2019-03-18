using System.Xml;
using Anathema.Player;
using UnityEngine;

namespace Anathema.Rooms
{
    public class JudasDoor : CutsceneDoor 
    {
        protected override bool LoadCutscene(GameObject player)
        {
            PlayerUpgrades upgrades = player.GetComponent<PlayerUpgrades>();

            if (upgrades.HasTalkedToJudas)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}