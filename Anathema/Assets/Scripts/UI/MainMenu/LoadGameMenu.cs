using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Anathema.Fsm;
using Anathema.Saving;
using Anathema.SceneLoading;

namespace Anathema.UI.MainMenu
{
    public class LoadGameMenu : SubMenu
    {
        [SerializeField] private string loadingScene;
        [SerializeField] private string playerScene;
        private string profileName;
        private bool loadStarted = false;
        public override void Enter()
        {
            base.Enter();
            var buttonList = menu.GetComponent<TextButtonList>();

            bool selectFirst = true;
            foreach (string profile in SaveProfile.GetProfileNames())
            {
                Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();
                onClick.AddListener(delegate{OnActivateProfile(profile);});

                TextButtonPreset preset = new TextButtonPreset(profile, selectFirst, onClick);

                buttonList.ButtonPresets.Add(preset);


                if (selectFirst)
                {
                    selectFirst = false;
                }
            }

            Button.ButtonClickedEvent onClick2 = new Button.ButtonClickedEvent();
            onClick2.AddListener(OnActivateMainMenu);
            TextButtonPreset preset2 = new TextButtonPreset("Menu", selectFirst, onClick2);

            buttonList.ButtonPresets.Add(preset2);

            buttonList.enabled = true;
        }
        public void OnActivateProfile(string profileName)
        {
            this.profileName = profileName;
            Invoke(nameof(LoadProfile), ClickDelay);
        }
        private void LoadProfile()
        {
            if (!loadStarted)
            {
                SaveProfile saveProfile = new SaveProfile(profileName);
                GameData gameData = saveProfile.Load();
                gameData.ProfileName = profileName;

                loadStarted = true;
                SceneLoader loader = new SceneLoader(loadingScene);
                loader.FadeScenes(this.gameObject.scene.name, playerScene, gameData);
            }
        }
        public override void Exit()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            base.Exit();
        }
    }
}