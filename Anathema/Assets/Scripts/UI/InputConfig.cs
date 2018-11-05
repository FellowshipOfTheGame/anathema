using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Anathema.UI
{
    public class InputConfig : MonoBehaviour
    {
        [SerializeField] private GameObject actionEditorPrefab;
        [SerializeField] private InputActionAsset actions;

        private void Start()
        {
            foreach (var action in actions.actionMaps[0])
            {
                var editor = Instantiate(actionEditorPrefab, this.transform).GetComponent<InputActionEditor>();
                if (editor) editor.Action = action;
            }
        }
        // private void Update()
        // {
        //    foreach(var key in Keyboard.current.allControls)
        //    {
        //         if (key == Keyboard.current.anyKey) continue;
        //         bool pressed = (float) key.ReadValueAsObject() != 0f;
        //         if (pressed)
        //             Debug.Log($"{key.displayName}: {pressed}");
        //    }
        // }
    }
}