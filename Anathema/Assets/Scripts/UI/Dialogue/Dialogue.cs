using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Malee;

namespace Anathema.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Anathema/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [Reorderable] public ReorderableDialogueList lines;
    }

    [System.Serializable]
    public class ReorderableDialogueList : ReorderableArray<DialogueLine> {}
}