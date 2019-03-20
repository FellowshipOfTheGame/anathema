using UnityEngine;
using Anathema.Fsm;

namespace Anathema.UI.MainMenu
{
    public class ExitMenu : SubMenu
    {
        public void OnActivateExit()
        {
            Invoke(nameof(ExitGame), ClickDelay);
        }
        private void ExitGame()
        {
            Application.Quit();
        }
    }
}