using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Anathema.Rooms;

namespace Anathema.SceneLoading
{
	public class SceneLoader
	{
        private string loadingScene;
        private string oldScene;
        private UniqueID destination;
        private GameObject player;
        public SceneLoader(string loadingScene)
        {
            this.loadingScene = loadingScene;
        }
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
                    break;
                }
            }
        }
        public void FadeScenes(string oldScene, UniqueID destination, GameObject player)
        {
            player?.SetActive(false);
            this.destination = destination;
            this.oldScene = oldScene;
            this.player = player;

            AsyncOperation loadingSceneLoadOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
            loadingSceneLoadOperation.completed += OnLoadingSceneLoaded;
        }
	}
}