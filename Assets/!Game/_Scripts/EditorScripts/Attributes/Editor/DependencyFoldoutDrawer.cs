using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(DependencyFoldoutAttribute))]
public class DependencyFoldoutDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        property.boolValue = EditorGUILayout.Foldout(property.boolValue, new GUIContent());
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }
}
