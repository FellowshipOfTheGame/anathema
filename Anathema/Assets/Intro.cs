using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

	[SerializeField] private Anathema.Dialogue.Dialogue introDialogue;
	private void Awake()
	{
		Anathema.Dialogue.DialogueHandler.instance.StartDialogue(introDialogue);
		Anathema.Dialogue.DialogueHandler.instance.OnDialogueEnd += EndIntro;
	}

	public void EndIntro()
	{
		GetComponent<Anathema.UI.CutsceneUnload>().LoadScene();
	}


}
