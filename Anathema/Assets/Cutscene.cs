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


	public void PlayNextCutscene(TimelineAsset timeline)
	{
		
	}

	private void Awake()
	{
		foreach(var action in actions)
		{
			actionQueue.Enqueue(action);
		}
		
	}

	 [System.Serializable]
    public class ReorderableEventList : ReorderableArray<UnityEvent> {}
}
