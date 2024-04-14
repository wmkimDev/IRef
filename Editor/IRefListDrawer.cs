using UnityEngine;
using UnityEditor;
using WMK.IRef.Runtime;

[CustomPropertyDrawer(typeof(IRefList<>), true)]
public class IRefListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty listProperty = property.FindPropertyRelative("targets");
        EditorGUI.PropertyField(position, listProperty, label, true);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty listProperty = property.FindPropertyRelative("targets");
        return EditorGUI.GetPropertyHeight(listProperty, label, true);
    }
}