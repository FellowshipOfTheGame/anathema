using Anathema.Rooms;

namespace Anathema.Saving
{
    [System.Serializable]
    public class GameData
    {
        public string ProfileName { get; set; } = "Debug";
        public bool hasScythe;
        public bool hasDoubleJump;
        public bool hasFireAttack;
        public bool hasTalkedToJudas;
        public int maxHealth;
        public UniqueID spawnLocation;
        public UniqueID[] keys;
        public UniqueID[] discoveredPickups;
    }
}