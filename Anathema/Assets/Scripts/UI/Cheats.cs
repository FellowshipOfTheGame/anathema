using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;
using Anathema.Rooms;

namespace Anathema.UI
{
    public class Cheats : MonoBehaviour
    {
        [SerializeField] private string loadingSceneName;
        private bool display = false;
        private string destination = "";
        private void OnGUI()
        {
            if (display)
            {
                GUI.Box(new Rect(10, 10, 355, 55), "Cheats!!!");
                destination = GUI.TextField(new Rect(25, 25, 250, 30), destination);
                if (GUI.Button(new Rect(280, 25, 60, 30), "Teleport") && !string.IsNullOrWhiteSpace(destination))
                {
                    Debug.Log(destination);
                    string[] names = destination.Split('.');
                    Debug.Log(names[0] + "    " + names[1]);
                    UniqueID destinationID = new UniqueID(names[0], names[1]);
                    destination = "";

                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    
                    if (player)
                    {
                        for (int i = 0; i < SceneManager.sceneCount; i++)
                        {
                            Scene scene = SceneManager.GetSceneAt(i);
                            if (scene != player.scene)
                            {
                                SceneLoader loader = new SceneLoader(loadingSceneName);
                                loader.FadeScenes(scene.name, destinationID, player);
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Quote))
            {
                display = !display;
            }
        }
    }
}