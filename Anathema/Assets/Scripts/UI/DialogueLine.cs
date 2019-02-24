using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Anathema.Dialogue
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Dialogue Properties")]
        [SerializeField] private string title;
        [SerializeField] [TextArea(3, 5)] private string text;

        [Space(10)]

        [Header("Dialogue Actions")]
        [SerializeField] private UnityEvent onEnterAction;
        [SerializeField] private UnityEvent onExitAction;

        public string Title { get { return title; } }
        public string Text { get { return text; } }
        public UnityEvent OnEnterAction { get { return onEnterAction; } }
        public UnityEvent OnExitAction { get { return onExitAction; } }
    }
}
