using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace wmk.IRef.Editor
{
    [CustomPropertyDrawer(typeof(IRef.Runtime.IRef<>), true)]
    public class IRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("target"), label);
            EditorGUI.EndProperty();
        }
    }
}