using UnityEngine;
using UnityEngine.EventSystems;

namespace Anathema.UI
{
    public class ForceSelectOnStart : MonoBehaviour
    {
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
    }
}