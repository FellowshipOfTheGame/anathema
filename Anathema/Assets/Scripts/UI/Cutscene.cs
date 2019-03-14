using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;
using Malee;

public class Cutscene : MonoBehaviour {

	[SerializeField] [Reorderable] private ReorderableEventList actions;
	[SerializeField] private PlayableDirector director;

	private Queue<UnityEvent> actionQueue = new Queue<UnityEvent>();

	public void PlayCutscene(TimelineAsset timeline)
	{
		director.Play(timeline);
		Invoke(nameof(PlayNext) , (float) timeline.duration);
	}

	private void Start()
	{
		foreach(var action in actions)
		{
			actionQueue.Enqueue(action);
		}
		PlayNext();
		Anathema.Dialogue.DialogueHandler.instance.OnDialogueEnd += PlayNext;
	}

	public void PlayNext()
	{
		if(actionQueue.Count != 0)
		{
			actionQueue.Dequeue().Invoke();
		}
	}

	private void OnDisable()
	{
		Anathema.Dialogue.DialogueHandler.instance.OnDialogueEnd -= PlayNext;
	}

	 [System.Serializable]
    public class ReorderableEventList : ReorderableArray<UnityEvent> {}
}
