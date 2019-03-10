using UnityEngine;
using System.Collections.Generic;
using Anathema.Saving;
using Anathema.Rooms;

namespace Anathema.Player
{
    public class PlayerUpgrades : MonoBehaviour
    {
        
        [Header("The default values for new saves are in the main menu scene.")]
        [Header("It is inteded for in editor testing")]
        [Header("This is the game data used when NOT starting from the menu.")]
        [SerializeField] private GameData gameData;
        public List<UniqueID> Keys { get; set; }
        private Health health;
        public bool HasScythe 
        {
            get
            {
                return gameData.hasScythe;
            }
            set
            {
                gameData.hasScythe = value;
            }
        }
        public bool HasDoubleJump 
        {
            get
            {
                return gameData.hasDoubleJump;
            }
            set
            {
                gameData.hasDoubleJump = value;
            }
        }
        public bool HasFireAttack 
        {
            get
            {
                return gameData.hasFireAttack;
            }
            set
            {
                gameData.hasFireAttack = value;
            }
        }
        public bool HasTalkedToJudas
        {
            get
            {
                return gameData.hasTalkedToJudas;
            }
            set
            {
                gameData.hasTalkedToJudas = value;
            }
        }
        public int MaxHealth 
        {
            get
            {
                return gameData.maxHealth;
            }
            set
            {
                gameData.maxHealth = value;
                health.MaxHP = value;
                health.Heal(value);
            }
        }
        public string ProfileName
        {
            get
            {
                return gameData.ProfileName;
            }
        }
        private void Start()
        {
            health = GetComponent<Health>();
            if (!health) Debug.LogWarning($"{gameObject.name}: {nameof(PlayerUpgrades)}: Requires a {nameof(Health)} component.");

            Keys = new List<UniqueID>(gameData.keys);
        }
        public void LoadData(GameData gameData)
        {
            this.gameData = gameData;

            Keys = new List<UniqueID>(gameData.keys);

            if (health)
            {
                health.MaxHP = MaxHealth;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: {nameof(PlayerUpgrades)}: Couldn't set MaxHP.");
            }
        }
        public GameData GetDataForSaving()
        {
            gameData.keys = Keys.ToArray();
            return gameData;
        }
    }
}