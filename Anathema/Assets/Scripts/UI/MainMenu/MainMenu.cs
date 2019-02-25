using UnityEngine;
using Anathema.SceneLoading;
using Anathema.Rooms;
using Anathema.Saving;
using Anathema.Fsm;

namespace Anathema.UI.MainMenu
{
    public class MainMenu : MenuState
    {
        public void OnActivateNewGame()
        {
            Invoke(nameof(OpenNewGame), ClickDelay);
        }
        private void OpenNewGame()
        {
            fsm.Transition<NewGameMenu>();
        }
        public void OnActivateLoadGame()
        {
            Invoke(nameof(OpenLoadGame), ClickDelay);
        }
        private void OpenLoadGame()
        {
            fsm.Transition<LoadGameMenu>();
        }
        public void OnActivateOptions()
        {
            Invoke(nameof(OpenOptions), ClickDelay);
        }
        private void OpenOptions()
        {
            // fsm.Transition<OptionsMenu>();
        }
        public void OnActivateCredits()
        {
            Invoke(nameof(OpenCredits), ClickDelay);
        }
        private void OpenCredits()
        {
            fsm.Transition<CreditsMenu>();
        }
        public void OnActivateExit()
        {
            Invoke(nameof(OpenExit), ClickDelay);
        }
        private void OpenExit()
        {
            fsm.Transition<ExitMenu>();
        }
    }
}