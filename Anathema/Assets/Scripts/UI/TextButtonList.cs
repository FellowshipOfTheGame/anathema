using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Malee;

public class TextButtonList : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] TextButtonTheme theme;
    [SerializeField] [Reorderable] private ReorderableTextButtonList buttonPresets;
    private void Start()
    {
        foreach (TextButtonPreset preset in buttonPresets)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, transform);
            preset.Apply(buttonObj, theme);
        }
    }
}
[System.Serializable] public class ReorderableTextButtonList : ReorderableArray<TextButtonPreset> {}