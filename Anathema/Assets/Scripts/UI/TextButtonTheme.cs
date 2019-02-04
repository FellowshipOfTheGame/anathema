using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TextButtonTheme", menuName = "Anathema/Text Button Theme")]
public class TextButtonTheme : ScriptableObject
{
    [SerializeField] private TextButtonStyle normal;
    [SerializeField] private TextButtonStyle highlighted;
    [SerializeField] private TextButtonStyle pressed;
    [SerializeField] private TextButtonStyle disabled;
    public TextButtonStyle Normal { get { return normal; } }
    public TextButtonStyle Highlighted { get { return highlighted; } }
    public TextButtonStyle Pressed { get { return pressed; } }
    public TextButtonStyle Disabled { get { return disabled; } }
}