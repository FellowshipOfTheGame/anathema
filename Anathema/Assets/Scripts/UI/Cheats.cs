using Anathema.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.SceneLoading;
using Anathema.Rooms;
using Anathema.Saving;

namespace Anathema.UI
{
    public class Cheats : MonoBehaviour
    {
        [SerializeField] private string loadingSceneName;
        private bool display = false;
        private string destination = "";
        private string newKey = "";
        private GameObject player;
        private PlayerUpgrades playerUpgrades;
        private Rect windowRect = new Rect(20,20 , 400, 200);

        private void Awake()
        {
            player = PlayerFinder.Find("Player");
            playerUpgrades = player.GetComponent<PlayerUpgrades>();
        }
        private void OnGUI()
        {
            if (display)
            {
                windowRect = GUILayout.Window(0, windowRect, DoConsole, "Console");
            }
        }

        private void DoConsole(int windowID)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    destination = GUILayout.TextField(destination);
                    if (GUILayout.Button("Teleport", GUILayout.MaxWidth(100)) &&
                        !string.IsNullOrWhiteSpace(destination))
                    {
                        Debug.Log(destination);
                        string[] names = destination.Split('.');
                        Debug.Log(names[0] + "    " + names[1]);
                        UniqueID destinationID = new UniqueID(names[0], names[1]);
                        destination = "";

                        if (player)
                        {
                            for (int i = 0; i < SceneManager.sceneCount; i++)
                            {
                                Scene scene = SceneManager.GetSceneAt(i);
                                if (scene != player.scene)
                                {
                                    SceneLoader loader = new SceneLoader(loadingSceneName);
                                    
                                    loader.ScenesToUnload.Add(gameObject.scene.name);
                                    loader.Destination = destinationID;
                                    loader.GameData = playerUpgrades.GetDataForSaving();
                                    
                                    loader.FadeScenes();
                                    break;
                                }
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
                
                playerUpgrades.HasDoubleJump = GUILayout.Toggle(playerUpgrades.HasDoubleJump, "Double Jump");
                playerUpgrades.HasScythe = GUILayout.Toggle(playerUpgrades.HasScythe, "Scythe");
                playerUpgrades.HasFireAttack = GUILayout.Toggle(playerUpgrades.HasFireAttack, "Fire Attack");
                playerUpgrades.HasTalkedToJudas = GUILayout.Toggle(playerUpgrades.HasTalkedToJudas, "Talked to Judas");

                foreach (var key in playerUpgrades.Keys)
                {
                    GUILayout.Label(key.ToString());
                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {
                        playerUpgrades.Keys.Remove(key);
                        break;
                    }
                }
                
                GUILayout.Label("Add Key:");
                newKey = GUILayout.TextField(newKey);
                if (GUILayout.Button("Add", GUILayout.MaxWidth(100)) &&
                    !string.IsNullOrWhiteSpace(newKey))
                {
                    string[] names = newKey.Split('.');
                    UniqueID keyID = new UniqueID(names[0], names[1]);
                    
                    playerUpgrades.Keys.Add(keyID);
                }
            }
            GUILayout.EndVertical();
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