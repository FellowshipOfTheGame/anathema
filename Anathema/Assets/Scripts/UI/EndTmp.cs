//This is just a simple hack, not a basis for a proper menu
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anathema.UI
{
    public class EndTmp : MonoBehaviour
    {
        private bool loadStarted = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) && !loadStarted)
            {
                loadStarted = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}