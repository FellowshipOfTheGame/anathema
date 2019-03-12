using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Cutscene : MonoBehaviour {

	[SerializeField] private List<TimelineAsset> timelines;
	private Queue<TimelineAsset> timelineQueue = new Queue<TimelineAsset>();
	[SerializeField] List<PlayableDirector> playables;


	public void PlayNext()
	{
		TimelineAsset currentTimeline = timelineQueue.Dequeue();

		foreach(var playable in playables)
		{
			playable.Play(currentTimeline);
		}
	}

	private void Awake()
	{
		foreach(var timeline in timelines)
		{
			timelineQueue.Enqueue(timeline);
		}

		PlayNext();
	}

}
