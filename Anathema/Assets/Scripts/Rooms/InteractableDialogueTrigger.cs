using UnityEngine;

namespace Anathema.Dialogue
{
    public class InteractableDialogueTrigger : MonoBehaviour
    {
        [SerializeField] protected GameObject interactionHint;
        [SerializeField] protected float hintPersistTime;
		[SerializeField] protected Dialogue dialogue;
        protected bool isInsideTrigger = false;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                CancelInvoke();
                interactionHint.SetActive(true);

                isInsideTrigger = true;
            }
        }

		protected virtual void Update()
        {
            if(isInsideTrigger && Input.GetButtonDown("Interact"))
            {
				DialogueHandler.instance.StartDialogue(dialogue);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInsideTrigger = false;
                Invoke(nameof(HideHint), hintPersistTime);
            }
        }
        private void HideHint()
        {
            interactionHint.SetActive(false);
        }
    }
}