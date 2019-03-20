using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anathema.Player
{
    public class PlayerFinder
    {
        public static GameObject Find(string playerScene)
        {
            Scene scene = SceneManager.GetSceneByName(playerScene);
            if (scene.IsValid())
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject root in rootObjects)
                {
                    if (root.CompareTag("Player"))
                    {
                        return root;
                    }
                }
                Debug.LogWarning($"PlayerFinder: Couldn't find confiner in scene {playerScene}");
            }
            else
            {
                Debug.LogWarning($"PlayerFinder: Couldn't find scene with name {playerScene}");
            }
            return null;
        }
    }  
}