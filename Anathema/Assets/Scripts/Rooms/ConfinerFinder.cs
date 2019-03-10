using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Anathema.Rooms
{
	public class ConfinerFinder : MonoBehaviour 
	{
		[SerializeField] private string cameraScene;
		private CinemachineConfiner confiner = null;
		private CinemachineConfiner Confiner
		{
			get
			{
				if (confiner)
				{
					return confiner;
				}
				
                Scene scene = SceneManager.GetSceneByName(cameraScene);
                if (scene.IsValid())
                {
                    GameObject[] rootObjects = scene.GetRootGameObjects();
                    
                    foreach (GameObject root in rootObjects)
                    {
                        CinemachineConfiner confiner = root.GetComponentInChildren<CinemachineConfiner>(true);
                        if (confiner)
                        {
                            this.confiner = confiner;
                            return confiner;
                        }
                    }
                    
                    Debug.LogWarning(
                        $"{gameObject.name}: {GetType()}: Couldn't find confiner in scene {cameraScene}");
                    return null;
                }
                
                Debug.LogWarning(
                    $"{gameObject.name}: {GetType()}: Couldn't find scene with name {cameraScene}");
                return null;
			}
		}

		public CompositeCollider2D ConfinerBounds
		{
			set
			{
				if (Confiner)
				{
                    Confiner.m_BoundingShape2D = value;
                    Confiner.InvalidatePathCache();
                }
			}
		}
	}
}