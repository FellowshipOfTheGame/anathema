using UnityEngine;

namespace Anathema.Dialogue
{
    public class InteractableDialogueTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private float hintPersistTime;
		[SerializeField] private Dialogue dialogue;
        private bool isInsideTrigger = false;
        
        protected void OnTriggerEnter2D(Collider2D other)
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
            if(isInsideTrigger && Input.GetKeyDown(KeyCode.E))
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