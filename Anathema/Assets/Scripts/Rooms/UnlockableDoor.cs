using UnityEngine;
using Anathema.Player;

namespace Anathema.Rooms
{
    public class UnlockableDoor : Door
    {
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private float hintPersistTime;
        private Collider2D player;
        private bool isInsideTrigger = false;
        protected virtual void Update()
        {
            if(isInsideTrigger && Input.GetKeyDown(KeyCode.E))
            {
                PlayerUpgrades playerUpgrades = player.GetComponent<PlayerUpgrades>();
                if (playerUpgrades.Keys.Exists(key => key.Equals(this.UniqueID)))
                {
                    base.OnTriggerEnter2D(player);
                }
                else
                {
                    Debug.Log("You don't have the needed key.");
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