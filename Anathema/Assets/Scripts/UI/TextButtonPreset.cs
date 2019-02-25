using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class TextButtonPreset
{
    [SerializeField] private bool useCustomTheme;
    [Anathema.HideInInspectorIfNot(nameof(useCustomTheme))][SerializeField] private TextButtonTheme customTheme;
    [SerializeField] private string text;
    [SerializeField] private bool disableInteraction;
    [SerializeField] private bool forceSelectOnStart;
    [SerializeField] private Button.ButtonClickedEvent onClick;
    public TextButtonPreset
    (
        string text, bool forceSelectOnStart,
        Button.ButtonClickedEvent onClick = null, bool disableInteraction = false,
        bool useCustomTheme = false, TextButtonTheme customTheme = null
    )
    {
        this.text = text;
        this.forceSelectOnStart = forceSelectOnStart;
        this.onClick = onClick;
        this.disableInteraction = disableInteraction;
        this.useCustomTheme = useCustomTheme;
        this.customTheme = customTheme;
    }
    public void Apply(GameObject buttonObj, TextButtonTheme defaultTheme = null)
    {
        if (forceSelectOnStart)
        {
            EventSystem.current.SetSelectedGameObject(buttonObj);
        } 

        var textMesh = buttonObj.GetComponent<TextMeshProUGUI>();

        textMesh.text = text;

        var button = buttonObj.GetComponent<Button>();
        button.onClick = onClick;
        button.interactable = !disableInteraction;

        var theme = useCustomTheme ? customTheme : defaultTheme;
        var animator = buttonObj.GetComponent<Animator>();
        var behaviours = animator.GetBehaviours<TextButtonBehaviour>();       
        foreach (var behaviour in behaviours)
        {
            switch(behaviour.ID)
            {
                case "Normal":
                    behaviour.Style = theme.Normal;
                break;
                case "Highlighted":
                    behaviour.Style = theme.Highlighted;
                break;
                case "Pressed":
                    behaviour.Style = theme.Pressed;
                break;
                case "Disabled":
                    behaviour.Style = theme.Disabled;
                break;
                //FIXME: proper error message
                default:
                    Debug.Log("unknown id");
                break;
            }
        }
    }
}