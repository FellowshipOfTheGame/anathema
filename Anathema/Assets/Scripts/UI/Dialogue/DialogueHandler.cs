using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Anathema.Dialogue
{
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

		private Queue<DialogueLine> dialogueLines = new Queue<DialogueLine>();
		private DialogueLine currentLine;
		private bool isLineDone;
		private bool isActive;

		public delegate void DialogueAction(Dialogue dialogue);
		public event DialogueAction OnDialogue;

		public static DialogueHandler instance;

		private void Awake()
		{
			if(instance == null)
				instance = this;
			else if(instance != this)
				Destroy(this);

			OnDialogue += StartDialogue;
		}
		
		public void StartDialogue()
		{
			if(isActive)
				EndDialogue();

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
				currentLine.OnEnterAction.Invoke();

				if(useTitles)
					titleText.text = currentLine.Title;

				yield return FillInText();

				isLineDone = true;

				if(autoSkip)
				{
					yield return new WaitForSecondsRealtime(timeUntilSkip);
					currentLine.OnExitAction.Invoke();
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
				{
					currentLine.OnExitAction.Invoke();
					StartCoroutine("NextLine");
				}
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
			titleText.text = "";
			StopAllCoroutines();
			currentLine = null;
			isActive = false;
			Time.timeScale = 1f;
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