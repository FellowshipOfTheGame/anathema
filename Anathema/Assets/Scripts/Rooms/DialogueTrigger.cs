using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Anathema.Dialogue
{
	public class DialogueTrigger : MonoBehaviour {

		[SerializeField] private Dialogue dialogue;

		private void OnTriggerEnter2D(Collider2D other)
		{
			DialogueHandler.instance.StartDialogue(dialogue);
		}

	}
}

