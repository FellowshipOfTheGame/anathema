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
        private string loadingScene;
        private string oldScene;
        private UniqueID destination;
        private GameObject player;
        private GameData gameData;
        private string playerScene;
        private bool reloadPlayerScene;

        public delegate void SceneUnloadHandler(string scene);
        public delegate void SceneLoadHandler(UniqueID destination, GameData gameData);

        public static SceneUnloadHandler OnSceneAboutToUnload;
        public static SceneLoadHandler OnSceneLoaded;
        public static SceneLoadHandler OnLateSceneLoaded;
        
        /// <summary>
        /// Creates a new SceneLoader using loadingScene as loading screen.
        /// </summary>
        /// <param name="loadingScene">Loading Screen name.</param>
        public SceneLoader(string loadingScene)
        {
            this.loadingScene = loadingScene;
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
                    swapScenes.OldScene = oldScene;
                    swapScenes.Destination = destination;
                    swapScenes.Player = player;
                    swapScenes.GameData = gameData;
                    swapScenes.PlayerScene = playerScene;
                    swapScenes.ReloadPlayerScene = reloadPlayerScene;
                    break;
                }
            }
        }
        /// <summary>
        /// Loads both the player scene and a new scene, with a fade to black transition effect.
        /// Intended for use in situations such as loading from the menu.
        /// </summary>
        /// <param name="oldScene">The scene to unload.</param>
        /// <param name="destination">The UniqueID of a destination UniqueComponent.</param>
        /// <param name="playerScene">The name of the Player scene to load.</param>
        /// <param name="reloadPlayerScene"> Whether to unload an exist Player scene.</param>
        public void FadeScenes(string oldScene, string playerScene, GameData gameData, bool reloadPlayerScene = false)
        {
            this.playerScene = playerScene;
            this.gameData = gameData;
            this.reloadPlayerScene = reloadPlayerScene;
            FadeScenes(oldScene, gameData.spawnLocation, (GameObject) null);
        }
        /// <summary>
        /// Loads a new scene with a fade to black transition effect.
        /// </summary>
        /// <param name="oldScene">The scene to unload.</param>
        /// <param name="destination">The UniqueID of a destination UniqueComponent.</param>
        /// <param name="player">The player's GameObject.</param>
        public void FadeScenes(string oldScene, UniqueID destination, GameObject player)
        {
            runningWithoutSceneLoader = false;

            player?.SetActive(false);
            this.destination = destination;
            this.oldScene = oldScene;
            this.player = player;

            //Request loading of loading screen.
            AsyncOperation loadingSceneLoadOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            //Registers OnLoadingSceneLoaded as listener to complete.
            loadingSceneLoadOperation.completed += OnLoadingSceneLoaded;
        }
	}
}