using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(ScanUIProfile))]

public class ScanUIProfileEditor : ExtendedEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        Space(10);
        DrawTitleLayout("Scan UI Profile");
        Space(10);
        
        DrawAllProperties();

        serializedObject.ApplyModifiedProperties();
    }
}