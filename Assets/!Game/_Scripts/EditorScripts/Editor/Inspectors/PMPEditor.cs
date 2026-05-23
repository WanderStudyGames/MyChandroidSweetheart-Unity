using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PlayerMovementProfile))]

public class PMPEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Runner");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        DrawTitleBlock(_icon, "Player Movement Profile");

        //DrawProperty("accelerationGround");
        //DrawProperty("accelerationAir");
        //DrawProperty("maxAcc");
        //DrawProperty("curMaxAcc");
        //DrawSeparator(offset: 7);
        //DrawProperty("friction");
        //DrawProperty("gravity");
        //DrawSeparator(offset: 7);
        //DrawProperty("jumpForce");
        //DrawProperty("jumpFalloffMultiplier");
        //DrawProperty("concreteStepSFX");
        DrawAllProperties();
        Space(10);
        serializedObject.ApplyModifiedProperties();
    }
}
