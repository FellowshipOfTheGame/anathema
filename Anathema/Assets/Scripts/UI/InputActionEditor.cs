using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.UI
{
    public class InputActionEditor : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject bindingEditorPrefab;
        [SerializeField] private TMPro.TextMeshProUGUI title;
        private InputAction action;
        public InputAction Action
        {
            get
            {
                return action;        
            }
            set
            {
                action = value;
                title.text = action.name;
                
                foreach (var binding in action.bindings)
                {
                    var editor = Instantiate(bindingEditorPrefab, content).GetComponent<InputBindingEditor>();
                    if (editor) editor.Binding = binding;
                }
            }
        }
        public void Rebind()
        {
            Action.PerformInteractiveRebinding();
        }
    }
}