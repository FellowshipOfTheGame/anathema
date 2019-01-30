using UnityEditor;
using UnityEngine;
 
namespace Anathema.Editor
{
    [CustomPropertyDrawer(typeof(HideInInspectorIf))]
    public class HideInInspectorIfDrawer : PropertyDrawer
    {
        private string ConditionName { get { return ((HideInInspectorIf) attribute).conditionName; } }
        private bool IsHidden(SerializedProperty property)
        {
            SerializedProperty condition = property.serializedObject.FindProperty(property.propertyPath.Replace(property.name, ConditionName));
            if (condition != null)
            {
                return condition.boolValue;
            }
            else
            {
                Debug.LogWarning($"{property.GetType()}.{property.name}: Can't find field with name: {ConditionName}");
                return false;
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log(((MonoBehaviour)property.serializedObject.targetObject).GetComponent<Collider2D>());
            if (!IsHidden(property))
            {
                EditorGUI.BeginProperty(position, label, property);
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
                EditorGUI.EndProperty();
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (IsHidden(property))
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
        }
    }
}