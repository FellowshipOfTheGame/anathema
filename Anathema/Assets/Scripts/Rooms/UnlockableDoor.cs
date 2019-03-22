using UnityEngine;
using Anathema.Player;
using Anathema.Dialogue;

namespace Anathema.Rooms
{
    public class UnlockableDoor : Door
    {
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private float hintPersistTime;
        [SerializeField] private Dialogue.Dialogue missingKey;
        [SerializeField] private Dialogue.Dialogue doorUnlock;
        
        private Collider2D player;
        private bool isInsideTrigger = false;
        protected virtual void Update()
        {
            if(isInsideTrigger && Input.GetKeyDown(KeyCode.E) && !DialogueHandler.instance.IsActive)
            {
                PlayerUpgrades playerUpgrades = player.GetComponent<PlayerUpgrades>();
                if (playerUpgrades.Keys.Exists(key => key.Equals(this.UniqueID)))
                {
                    DialogueHandler.instance.StartDialogue(doorUnlock);
                    base.OnTriggerEnter2D(player);
                }
                else
                {
                    DialogueHandler.instance.StartDialogue(missingKey);
                }
            }
        }
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CancelInvoke();
                interactionHint.SetActive(true);

                player = other;
                isInsideTrigger = true;
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInsideTrigger = false;
                player = null;
                Invoke(nameof(HideHint), hintPersistTime);
            }
        }
        private void HideHint()
        {
            interactionHint.SetActive(false);
        }
    }
}