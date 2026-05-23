using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(ScannerProfile))]

public class ScannerProfileEditor : ExtendedEditor
{
    Texture _icon;
    
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Scanner");
        
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        DrawTitleBlock(_icon, "Scanner Profile");
        DrawAllProperties();
        serializedObject.ApplyModifiedProperties();
    }
}