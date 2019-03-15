using UnityEngine;
using Anathema.Player;
using Anathema.Saving;
using Anathema.Dialogue;
using Anathema.SceneLoading;

namespace Anathema.Rooms
{
    public class SaveStation : UniqueTrigger
    {
        private bool saved = false;
        [SerializeField] private Dialogue.Dialogue saveDialogue;
        protected override void OnTriggerActivate(Collider2D other)
        {
            if (!saved && other.CompareTag("Player"))
            {
                PlayerUpgrades upgrades = other.GetComponent<PlayerUpgrades>();

                if (upgrades)
                {
                    GameData gameData = upgrades.GetDataForSaving();
                    if (gameData != null)
                    {
                        DialogueHandler.instance.StartDialogue(saveDialogue);
                        
                        saved = true;

                        gameData.spawnLocation = this.UniqueID;

                        SaveProfile saveProfile = new SaveProfile(gameData.ProfileName, true);
                        saveProfile.Save(gameData);
                    }
                    else
                    {
                        Debug.LogWarning("SaveStation: " + nameof(GameData) + " is null.");
                    }
                }
                else
                {
                    Debug.LogWarning("SaveStation: Player doesn't have " + nameof(PlayerUpgrades) + " component.");
                }
                
                Health health = other.GetComponent<Health>();

                if (health)
                {
                    health.Heal(health.MaxHP);
                }
                else
                {
                    Debug.LogWarning("SaveStation: Player doesn't have" + nameof(Health) + " component.");
                }
            }
        }
    }
}