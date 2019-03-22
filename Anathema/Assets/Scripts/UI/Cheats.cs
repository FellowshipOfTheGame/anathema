using System;
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
        private UniqueID destination = new UniqueID();
        private UniqueID newKey = new UniqueID();
        private GameObject player;
        private PlayerUpgrades playerUpgrades;
        private Health health;
        private string newHP = "";
        private string newMaxHP = "";
        private Rect windowRect = new Rect(20,20 , 400, 200);
        
        private void Awake()
        {
            player = PlayerFinder.Find("Player");
            playerUpgrades = player.GetComponent<PlayerUpgrades>();
            health = player.GetComponent<Health>();
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
                    destination = new UniqueID(GUILayout.TextField(destination.SceneName), GUILayout.TextField(destination.ObjectName));
                    
                    if (GUILayout.Button("Teleport", GUILayout.MaxWidth(100)) &&
                        !string.IsNullOrWhiteSpace(destination.SceneName) &&
//                        SceneManager.GetSceneByName(destination.SceneName).IsValid() &&
                        !string.IsNullOrWhiteSpace(destination.ObjectName))
                    {
                        if (player)
                        {
                            for (int i = 0; i < SceneManager.sceneCount; i++)
                            {
                                Scene scene = SceneManager.GetSceneAt(i);
                                if (scene != player.scene)
                                {
                                    SceneLoader loader = new SceneLoader(loadingSceneName);
                                    
                                    loader.ScenesToUnload.Add(scene.name);
                                    loader.Destination = destination;
                                    loader.GameData = playerUpgrades.GetDataForSaving();
                                    
                                    loader.FadeScenes();
                                    break;
                                }
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Health: ");
                    newHP = GUILayout.TextField(newHP);
                    if (GUILayout.Button("Set", GUILayout.Width(100)))
                    {
                        health.Hp = Int32.Parse(newHP);
                    }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                    GUILayout.Label("MaxHealth: ");
                    newMaxHP = GUILayout.TextField(newMaxHP);
                    if (GUILayout.Button("Set", GUILayout.Width(100)))
                    {
                        playerUpgrades.MaxHealth = Int32.Parse(newMaxHP);
                    }
                GUILayout.EndHorizontal();
                health.isInvulnerable = GUILayout.Toggle(health.isInvulnerable, "Invunerable");
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
                newKey = new UniqueID(GUILayout.TextField(newKey.SceneName), GUILayout.TextField(newKey.ObjectName));
                if (GUILayout.Button("Add", GUILayout.MaxWidth(100)) &&
                    !string.IsNullOrWhiteSpace(newKey.SceneName) &&
//                    SceneManager.GetSceneByName(newKey.SceneName).IsValid() &&
                    !string.IsNullOrWhiteSpace(newKey.ObjectName))
                {
                    playerUpgrades.Keys.Add(newKey);
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