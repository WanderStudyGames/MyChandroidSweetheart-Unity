using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(SprocketProfile))]
public class SPPEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Sprocket");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        DrawTitleBlock(_icon, "Sprocket Profile");
        DrawAllProperties();
        serializedObject.ApplyModifiedProperties();
    }
}