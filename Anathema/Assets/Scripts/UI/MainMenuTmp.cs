//This is just a simple hack, not a basis for a proper menu
using UnityEngine;
using Anathema.SceneLoading;
using Anathema.Rooms;

namespace Anathema.UI
{
    public class MainMenuTmp : MonoBehaviour
    {
        [SerializeField] private UniqueID destination;
        [SerializeField] private string loadingScene;
        [SerializeField] private string playerScene;
        private bool loadStarted = false;
        public void Load()
        {
            if (!loadStarted)
            {
                loadStarted = true;
                SceneLoader loader = new SceneLoader(loadingScene);
                loader.FadeScenes(this.gameObject.scene.name, destination, playerScene);
            }
        }
    }
}