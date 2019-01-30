using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Anathema.Rooms
{
	[RequireComponent(typeof(Collider2D))]
	public class AutoSetupCamBounds : MonoBehaviour 
	{
		[SerializeField] private string cameraScene;
		private void OnEnable()
		{
			Collider2D collider2D = GetComponent<Collider2D>();
			if (!collider2D)
			{
				Debug.LogWarning($"{gameObject.name}: {GetType()}: Requires an Collider2D component.");
				return;
			}
			
			Scene scene = SceneManager.GetSceneByName(cameraScene);
			if (scene.IsValid())
			{
				bool foundConfiner = false;
				GameObject[] rootObjects = scene.GetRootGameObjects();
				foreach (GameObject root in rootObjects)
				{
					CinemachineConfiner confiner = root.GetComponentInChildren<CinemachineConfiner>(true);	
					if (confiner)
					{
						confiner.m_BoundingShape2D = collider2D;
						confiner.InvalidatePathCache();
						foundConfiner = true; 
					}
				}
				if (!foundConfiner)
				{
					Debug.LogWarning($"{gameObject.name}: {GetType()}: Couldn't find confiner in scene {cameraScene}");
				}
			}
			else
			{
				Debug.LogWarning($"{gameObject.name}: {GetType()}: Couldn't find scene with name {cameraScene}");
			}
		}
	}
}