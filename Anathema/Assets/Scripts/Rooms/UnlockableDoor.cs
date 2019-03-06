using UnityEngine;
using Anathema.Player;

namespace Anathema.Rooms
{
    public class UnlockableDoor : Door
    {
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private float hintPersistTime;
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CancelInvoke();
                interactionHint.SetActive(true);

                if(Input.GetKeyDown(KeyCode.E))
                {
                    PlayerUpgrades playerUpgrades = other.GetComponent<PlayerUpgrades>();
                    if (playerUpgrades.HasKey)
                    {
                        base.OnTriggerEnter2D(other);
                    }
                    else
                    {
                        //Display message
                    }
                }
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Invoke(nameof(HideHint), hintPersistTime);
            }
        }
        private void HideHint()
        {
            interactionHint.SetActive(false);
        }
    }
}