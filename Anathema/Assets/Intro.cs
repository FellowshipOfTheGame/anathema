using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

	[SerializeField] private Anathema.Dialogue.Dialogue introDialogue;
	[SerializeField] private float awakeRetryRate = 0.05f;
	private void Awake()
	{
		if (Anathema.Dialogue.DialogueHandler.instance)
		{
            Anathema.Dialogue.DialogueHandler.instance.StartDialogue(introDialogue);
            Anathema.Dialogue.DialogueHandler.instance.OnDialogueEnd += EndIntro;
		}
		else
		{
			Invoke(nameof(Awake), awakeRetryRate);
		}
	}

	public void EndIntro()
	{
		GetComponent<Anathema.UI.CutsceneUnload>().LoadScene();
	}


}
