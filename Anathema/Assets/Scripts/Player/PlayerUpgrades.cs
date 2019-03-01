using UnityEngine;
using Anathema.Saving;

namespace Anathema.Player
{
    public class PlayerUpgrades : MonoBehaviour
    {
        
        [Header("The default values for new saves are in the main menu scene.")]
        [Header("It is inteded for in editor testing")]
        [Header("This is the game data used when NOT starting from the menu.")]
        [SerializeField] private GameData gameData;
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
        public int MaxHealth 
        {
            get
            {
                return gameData.maxHealth;
            }
            set
            {
                gameData.maxHealth = value;
            }
        }
        public void LoadData(GameData gameData)
        {
            this.gameData = gameData;
        }
        public GameData GetDataForSaving()
        {
            return gameData;
        }
    }
}