using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Malee;

public class TextButtonList : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] TextButtonTheme theme;
    [SerializeField] [Reorderable] private ReorderableTextButtonList buttonPresets;
    public ReorderableTextButtonList ButtonPresets
    {
        get { return buttonPresets; }
        set { buttonPresets = value; }
    }
    private void OnEnable()
    {
        foreach (TextButtonPreset preset in buttonPresets)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, transform);
            preset.Apply(buttonObj, theme);
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
[System.Serializable] public class ReorderableTextButtonList : ReorderableArray<TextButtonPreset> {}