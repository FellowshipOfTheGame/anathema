using UnityEngine;
using Anathema.Player;
using Anathema.Saving;
using Anathema.Dialogue;

namespace Anathema.Rooms
{
    public class SaveStation : UniqueComponent
    {
        private bool saved = false;
        [SerializeField] private Dialogue.Dialogue saveDialogue;
        private void OnTriggerEnter2D(Collider2D other)
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
                        Debug.Log("SaveStation: " + nameof(GameData) + " is null.");
                    }
                }
                else
                {
                    Debug.Log("SaveStation: Player doesn't have " + nameof(PlayerUpgrades) + " component.");
                }
            }
        }
    }
}