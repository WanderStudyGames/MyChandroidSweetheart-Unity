using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
//[CustomEditor(typeof(ScanUI))]

public class ScanUIEditor : ExtendedEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawAllProperties();
        serializedObject.ApplyModifiedProperties();
        //base.OnInspectorGUI();
    }
}