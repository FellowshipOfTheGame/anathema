using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Anathema.Dialogue
{
	/// <summary>
	/// 	This is the main, manager class for a dialogue box system
	/// 	The pipeline is:
	/// 		-> StartDialogue() will start the dialogue from the "dialogue" variable and StartDialogue(dialogue) will start the dialogue from the parameter
	/// 		-> Skip() will skip to the next line, this has to be set manually, for example a button that calls this function, or automatically from the "autoSkip" setting
	/// 		-> EndDialogue() will be called when there are no more lines to be shown, but you can call it before to abruptly stop the dialogue
	/// </summary>
	public class DialogueHandler : MonoBehaviour
	{

		[Header("References")]
		[Tooltip("Reference to the TMPro text component of the main dialogue box.")]
		[SerializeField] private TextMeshProUGUI dialogueText;
		[Tooltip("Whether or not the dialogue has a title or character name display.")]
		[SerializeField] private bool useTitles;
		[Tooltip("Reference to the TMPro text component of the title/name display.")]
		[SerializeField] [HideInInspectorIfNot(nameof(useTitles))] private TextMeshProUGUI titleText;
		[Tooltip("Current dialogue script to be displayed. To create a new dialogue, go to Assets->Create->Anathema->Dialogue.")]
		[SerializeField] private Dialogue dialogue;
		[Tooltip("Game object that contains the chat box to be enabled/disabled")]
		[SerializeField] private GameObject dialogueBox;

		[Space(10)]

		[Header("Settings")]
		[Tooltip("Whether or not the characters are going to be displayed one at a time.")]
		[SerializeField] private bool useTypingEffect;
		[SerializeField] [HideInInspectorIfNot(nameof(useTypingEffect))] [Range(1, 60)] private int framesBetweenCharacters;
		[Tooltip("If true, trying to skip dialogue will first fill in the entire dialogue line and then skip if prompted again, if false it will skip right away.")]
		[SerializeField] [HideInInspectorIfNot(nameof(useTypingEffect))] private bool fillInBeforeSkip;
		[Tooltip("Whether or not, after filling in the entire text, the dialogue skips to the next line automatically.")]
		[SerializeField] private bool autoSkip;
		[SerializeField] [HideInInspectorIfNot(nameof(autoSkip))] private float timeUntilSkip;
        [Tooltip("Whether or not to pause game during dialogue")]
		[SerializeField] private bool pauseDuringDialogue;
        [Tooltip("Advanced setting: If there is only 1 handler/dialogue box (A visual novel for example) you can make this a singleton and call it from DialogueHandler.instance. If unsure, leave it false.")]
        [SerializeField] private bool isSingleton;
        

		private Queue<DialogueLine> dialogueLines = new Queue<DialogueLine>();
		private DialogueLine currentLine;
		private bool isLineDone;

		private bool isActive;
		public bool IsActive { get { return isActive; }	}
		
		public delegate void DialogueAction();
		public event DialogueAction OnDialogueStart;
		public event DialogueAction OnDialogueEnd;

		public static DialogueHandler instance;

		private void Awake()
		{
			if(isSingleton)
			{
				if(instance == null)
					instance = this;
				else if(instance != this)
					Destroy(this);
			}
		}
		
		public void StartDialogue()
		{
			OnDialogueStart?.Invoke();

			if(isActive)
				EndDialogue();

            if(pauseDuringDialogue)
			    Time.timeScale = 0f;

			foreach(var line in dialogue.lines)
				dialogueLines.Enqueue(line);

			isActive = true;
			dialogueBox.SetActive(true);
			StartCoroutine("NextLine");

		}

		public void StartDialogue(Dialogue dialogue)
		{
			this.dialogue = dialogue;
			StartDialogue();
		}

		private IEnumerator NextLine()
		{
			isLineDone = false;
			
			if(dialogueLines.Count != 0)
			{
				currentLine = dialogueLines.Dequeue();
				dialogueText.text = "";

				if(useTitles)
					titleText.text = currentLine.Title;

				yield return FillInText();

				isLineDone = true;

				if(autoSkip)
				{
					yield return new WaitForSecondsRealtime(timeUntilSkip);
					StartCoroutine("NextLine");
				}
			}
			else
				EndDialogue();
		}

		public void Skip()
		{
			if(isActive)
			{
				if(fillInBeforeSkip && !isLineDone)
				{
					StopAllCoroutines();
					dialogueText.text = currentLine.Text;
					isLineDone = true;
				}
				else
					StartCoroutine("NextLine");
			}
		}

		private IEnumerator FillInText()
		{
			if(useTypingEffect)
			{
				foreach(var character in currentLine.Text)
				{
					dialogueText.text += character;
					yield return WaitForFrames(framesBetweenCharacters);
				}
			}
			else
				dialogueText.text = currentLine.Text;
		}

		public void EndDialogue()
		{
			dialogueBox.SetActive(false);

			dialogueText.text = "";

			if(useTitles)
				titleText.text = "";

			StopAllCoroutines();

			currentLine = null;
			isActive = false;

            if(pauseDuringDialogue)
                Time.timeScale = 1f;

			OnDialogueEnd?.Invoke();
		}

		public static IEnumerator WaitForFrames(int frameCount)
		{
			while (frameCount > 0)
			{
				frameCount--;
				yield return null;
			}
		}

	}
	
}