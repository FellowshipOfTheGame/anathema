using System;
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
        [SerializeField] private string introStartScene;
        private bool loadStarted = false;
        private bool isIntroEnabled = true;
        public bool IsIntroEnabled
        {
            get {   return isIntroEnabled;  }
            set 
            {
                isIntroEnabled = value;
            }
        }
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

                if (isIntroEnabled)
                {
                   loader.ScenesToLoad.Add(introStartScene); 
                }
                else
                {
                    loader.ScenesToLoad.Add(playerScene);
                    loader.Destination = defaultGameData.spawnLocation;
                }
                                        
                loader.GameData = defaultGameData;
                
                try
                {
                    loader.FadeScenes();
                }
                catch (InvalidOperationException e)
                {
                    loadStarted = false;
                }
            }
        }
    }
}