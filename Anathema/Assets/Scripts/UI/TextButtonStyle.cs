using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextButtonStyle
{
    [SerializeField] private Color textColor;
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;
    public Color TextColor { get { return textColor; } }
    public string Prefix { get { return prefix; } }
    public string Suffix { get { return suffix; } }
}