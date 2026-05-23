using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DependencyAttribute))]
public class DependencyEditor : MonoBehaviourEditor
{
    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField("IT WORKED");
        base.OnInspectorGUI();
    }
}
