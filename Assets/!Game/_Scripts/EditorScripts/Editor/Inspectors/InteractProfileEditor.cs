using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(InteractProfile))]

public class InteractProfileEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Interact");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        DrawTitleBlock(_icon, "Interact Profile");
        
        DrawAllProperties();
        serializedObject.ApplyModifiedProperties();
    }
}