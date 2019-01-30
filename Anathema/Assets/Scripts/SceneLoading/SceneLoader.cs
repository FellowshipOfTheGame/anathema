using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Rooms;

namespace Anathema.SceneLoading
{
    /// <summary>
    /// SceneLoader is the interface to the Scene Loading System.
    /// <see cref="Door"> for example on use.
    /// </summary>
	public class SceneLoader
	{
        private string loadingScene;
        private string oldScene;
        private UniqueID destination;
        private GameObject player;
        private string playerScene;
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
                    swapScenes.PlayerScene = playerScene;
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
        public void FadeScenes(string oldScene, UniqueID destination, string playerScene)
        {
            this.playerScene = playerScene;
            FadeScenes(oldScene, destination, (GameObject) null);
        }
        /// <summary>
        /// Loads a new scene with a fade to black transition effect.
        /// </summary>
        /// <param name="oldScene">The scene to unload.</param>
        /// <param name="destination">The UniqueID of a destination UniqueComponent.</param>
        /// <param name="player">The player's GameObject.</param>
        public void FadeScenes(string oldScene, UniqueID destination, GameObject player)
        {
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