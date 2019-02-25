using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Anathema.UI
{
    /// <summary>
    /// <see cref="Refocus"/> tries to keep UI objects selected even when mouse steals focus.
    /// Use it in EVERY menu.
    /// </summary>
    public class Refocus : MonoBehaviour
    {
        private GameObject lastSelected;
        private void Update()
        {
            EventSystem sys = EventSystem.current;
            if (sys.currentSelectedGameObject == null)
            {
                if (lastSelected)
                {
                    sys.SetSelectedGameObject(lastSelected);
                }
                else if (sys.firstSelectedGameObject)
                {
                    sys.SetSelectedGameObject(sys.firstSelectedGameObject);
                }
                else
                {
                    Selectable selectable = Object.FindObjectOfType<Selectable>();
                    if (selectable != null)
                    {
                        sys.SetSelectedGameObject(selectable.gameObject);
                    }
                    else
                    {
                        Debug.Log("Refocus failed :(");
                    }
                }
            }
        }
    }
}