using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Dialogue
{
	public class LibraryBook : InteractableDialogueTrigger
	{
		[Tooltip("After reading the book for the first time")]
		[SerializeField] private Dialogue afterFirstReadDialogue;

		private bool hasRead;
		private Anathema.Player.PlayerUpgrades upgrades;
		private static GameObject player;

		private static int booksRead;
		private static int BooksRead
		{
			get {	return booksRead;	}
			set
			{
				Debug.Log("New value = " + value);
				if(value >= 3)
				{
					DialogueHandler.instance.OnDialogueEnd += LibraryCutscene;
					booksRead = 0;
				}
				booksRead = value;
			}
		}

		// Update is called once per frame
		protected override void Update ()
		{
			if(isInsideTrigger && !DialogueHandler.instance.IsActive &&Input.GetKeyDown(KeyCode.E))
            {
				if(!hasRead && !upgrades.HasDoubleJump)
				{
					DialogueHandler.instance.StartDialogue(dialogue);
					hasRead = true;
					BooksRead++;
				}
				else
					DialogueHandler.instance.StartDialogue(afterFirstReadDialogue);
            }
		}

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);
			if(other.tag == "Player")
			{
				upgrades = other.GetComponent<Anathema.Player.PlayerUpgrades>();
				player = other.gameObject;
			}
		}

		private static void LibraryCutscene()
		{
			Anathema.Player.CutsceneState playerState;

			playerState = player.GetComponent<Anathema.Player.CutsceneState>();

			playerState.StartCutscene(Anathema.Player.CutsceneState.UpgradeType.DoubleJump, true);

			DialogueHandler.instance.OnDialogueEnd -= LibraryCutscene;
		}
	}
}