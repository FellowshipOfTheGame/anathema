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
                saveProfile.Save(defaultGameData);

                loadStarted = true;
                SceneLoader loader = new SceneLoader(loadingScene);
                
                loader.ScenesToUnload.Add(gameObject.scene.name);
                loader.ScenesToLoad.Add(playerScene);
                loader.Destination = defaultGameData.spawnLocation;
                loader.GameData = defaultGameData;
                
                loader.FadeScenes();
            }
        }
        public void SetDestination(string sceneName, string objectName)
        {
            defaultGameData.spawnLocation = new Rooms.UniqueID(sceneName, objectName);
        }
    }
}