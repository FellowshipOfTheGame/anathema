using System;
using System.IO;
using Anathema.Rooms;
using Anathema.Saving;
using Anathema.SceneLoading;
using UnityEngine;

namespace Anathema.UI
{
    public class CutsceneUnload : MonoBehaviour
    {
        [SerializeField] private UniqueID destination;
        [SerializeField] private string loadingSceneName;
        [SerializeField] private string playerSceneName;
        [SerializeField] private bool loadPlayer = true;
        [SerializeField] private float loadSceneRetryTime = 0.05f;
        private GameData gameData;
        private void Awake()
        {
            SceneLoader.OnSceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(UniqueID destination, GameData gameData)
        {
            this.gameData = gameData;
        }

        public void LoadScene()
        {
            SceneLoader loader = new SceneLoader(loadingSceneName);
            
            loader.ScenesToUnload.Add(gameObject.scene.name);
            if (loadPlayer)
                loader.ScenesToLoad.Add(playerSceneName);
            loader.Destination = destination;
            loader.GameData = gameData;
            
            try
            {
                loader.FadeScenes();
            }
            catch (InvalidOperationException e)
            {
                Invoke(nameof(LoadScene), loadSceneRetryTime);
            }
        }

        private void OnDestroy()
        {
            SceneLoader.OnSceneLoaded -= SceneLoaded;
        }
    }
}