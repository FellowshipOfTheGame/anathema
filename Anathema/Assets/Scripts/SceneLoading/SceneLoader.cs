using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Rooms;
using Anathema.Saving;

namespace Anathema.SceneLoading
{
    /// <summary>
    /// SceneLoader is the interface to the Scene Loading System.
    /// <see cref="Door"> for example on use.
    /// </summary>
	public class SceneLoader
	{
        /// <summary>
        /// This variable is used for detecting if the current scene was loaded by the UnityEditor
        /// It is set to false whenever the SceneLoader is called.
        /// It is NOT RESET when loading a scene by other means.
        /// It is only meant for detecting an initial load by the UnityEditor
        /// </summary>
        public static bool runningWithoutSceneLoader = true;


        public delegate void SceneUnloadHandler(string scene);
        public delegate void SceneLoadHandler(UniqueID destination, GameData gameData);

        public static SceneUnloadHandler OnSceneAboutToUnload;
        public static SceneLoadHandler OnSceneLoaded;
        public static SceneLoadHandler OnLateSceneLoaded;

        public static bool CurrentlyLoading
        {
            get
            {
                if (string.IsNullOrWhiteSpace(loadingScene))
                    return false;
                
                var loading = SceneManager.GetSceneByName(loadingScene).isLoaded;
                
                return loading;
            }
        }
            
        private static string loadingScene;
        
        public List<string> ScenesToUnload { get; set; } = new List<string>();
        public List<string> ScenesToLoad { get; set; } = new List<string>();
        public UniqueID Destination { get; set;  } = null;
        public GameData GameData { get; set; } = null;
        
        
        /// <summary>
        /// Creates a new SceneLoader using loadingScene as loading screen.
        /// </summary>
        /// <param name="loadingScene">Loading Screen name.</param>
        public SceneLoader(string loadingScene)
        {
            SceneLoader.loadingScene = loadingScene;
        }
        /// <summary>
        /// Forwards the needed data to the SwapScenes state of the loading scene.
        /// </summary>
        private void OnLoadingSceneLoaded(AsyncOperation operation)
        {
            GameObject[] rootObjets = SceneManager.GetSceneByName(loadingScene).GetRootGameObjects();
            foreach (GameObject root in rootObjets)
            {
                SwapScenes swapScenes = root.GetComponentInChildren<SwapScenes>(true);
                if (swapScenes)
                {
                    swapScenes.Destination = Destination;
                    swapScenes.GameData = GameData;
                    swapScenes.ScenesToLoad = ScenesToLoad;
                    swapScenes.ScenesToUnload = ScenesToUnload;
                    break;
                }
            }
        }
        /// <summary>
        /// Loads a new scene with a fade to black transition effect.
        /// </summary>
        /// <param name="oldScene">The scene to unload.</param>
        /// <param name="destination">The UniqueID of a destination UniqueComponent.</param>
        /// <param name="player">The player's GameObject.</param>
        public void FadeScenes()
        {
            if (CurrentlyLoading)
                CurrentlyLoadingSceneError();
                
            if ((ScenesToLoad == null || ScenesToLoad.Count == 0) && Destination == null)
               NoSceneToLoadError();

            if (Destination != null)
            {
                if (ScenesToLoad == null)
                {
                    ScenesToLoad = new List<string>();
                    ScenesToLoad.Add(Destination.SceneName);
                }
                else if (!ScenesToLoad.Contains(Destination.SceneName))
                {
                    ScenesToLoad.Add(Destination.SceneName);
                }
            }

            runningWithoutSceneLoader = false;
            
            foreach (var scene in ScenesToUnload)
            {
                OnSceneAboutToUnload?.Invoke(scene);
            }
            //Request loading of loading screen.
            AsyncOperation loadingSceneLoadOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            //Registers OnLoadingSceneLoaded as listener to complete.
            loadingSceneLoadOperation.completed += OnLoadingSceneLoaded;
        }
        
        private void NoSceneToLoadError()
        {
            throw new InvalidOperationException("No scenes to load. Set either ScenesToLoad or Destination.");
        }

        private void CurrentlyLoadingSceneError()
        {
            throw new InvalidOperationException("Scene loader is currently loading a scene.");
        }
	}
}