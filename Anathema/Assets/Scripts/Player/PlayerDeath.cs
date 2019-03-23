using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Anathema.Saving;
using Anathema.SceneLoading;

namespace Anathema.Player
{
    public class PlayerDeath : Death
    {
        [SerializeField] private string loadingScene;
        [SerializeField] private string playerScene;
        [SerializeField] private float loadSceneRetryTime = 0.05f;
        private PlayerUpgrades playerUpgrades;
        protected override void Start()
        {
            base.Start();

            playerUpgrades = GetComponent<PlayerUpgrades>();
        }
        protected override void HandleBurnComplete()
        {
            ReloadSave();
        }
        private void ReloadSave()
        {
            SaveProfile saveProfile = new SaveProfile(playerUpgrades.ProfileName);
            GameData gameData = saveProfile.Load();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                
                if (scene.name != playerScene)
                {
                    SceneLoader loader = new SceneLoader(loadingScene);
                    
                    loader.ScenesToUnload.Add(playerScene);
                    loader.ScenesToUnload.Add(scene.name);
                    loader.ScenesToLoad.Add(playerScene);
                    loader.Destination = gameData.spawnLocation;
                    loader.GameData = gameData;
                    
                    try
                    {
                        loader.FadeScenes();
                    }
                    catch (InvalidOperationException e)
                    {
                        Invoke(nameof(ReloadSave), loadSceneRetryTime);
                    }
                               
                    break;
                }
            }
        }
    }
}