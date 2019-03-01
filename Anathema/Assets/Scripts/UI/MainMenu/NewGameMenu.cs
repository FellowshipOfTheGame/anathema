using UnityEngine;
using Anathema.Fsm;
using Anathema.Saving;
using Anathema.SceneLoading;

namespace Anathema.UI.MainMenu
{
    public class NewGameMenu : SubMenu
    {
        [SerializeField] private GameData defaultGameData;
        [SerializeField] private string loadingScene;
        [SerializeField] private string playerScene;
        [SerializeField] private TMPro.TextMeshProUGUI textMesh;
        private bool loadStarted = false;
        public void OnActivateStart()
        {
            Invoke(nameof(CreateGame), ClickDelay);
        }
        private void CreateGame()
        {
            if (!loadStarted)
            {
                if (string.IsNullOrWhiteSpace(textMesh.text))
                {
                    textMesh.text = "Invalid name";
                    return;
                }
       
                SaveProfile saveProfile = new SaveProfile(textMesh.text);
                defaultGameData.ProfileName = textMesh.text;

                loadStarted = true;
                SceneLoader loader = new SceneLoader(loadingScene);
                loader.FadeScenes(this.gameObject.scene.name, playerScene, defaultGameData);
            }
        }
    }
}