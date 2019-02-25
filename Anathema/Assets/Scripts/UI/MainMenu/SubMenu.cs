using UnityEngine;
using Anathema.Fsm;

namespace Anathema.UI.MainMenu
{
    public class SubMenu : MenuState
    {
        public void OnActivateMainMenu()
        {
            Invoke(nameof(OpenMainMenu), ClickDelay);
        }
        private void OpenMainMenu()
        {
            fsm.Transition<MainMenu>();
        }
    }
}