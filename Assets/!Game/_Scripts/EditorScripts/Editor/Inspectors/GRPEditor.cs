using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(GrappleProfile))]

public class GRPEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Grapple");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        DrawTitleBlock(_icon, "Grapple Profile");
        
        DrawAllProperties();
        serializedObject.ApplyModifiedProperties();
    }
}