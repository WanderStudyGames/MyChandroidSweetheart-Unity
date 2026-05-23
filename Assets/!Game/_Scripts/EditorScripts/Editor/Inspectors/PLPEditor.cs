using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(PlayerLookProfile))]

public class PLPEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Mouse");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }

        DrawTitleBlock(_icon, "Player Look Profile");
        
        DrawAllProperties();

        serializedObject.ApplyModifiedProperties();
    }
}