using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : ExtendedEditor
{
    //Texture _grapple;
    //Texture _sprocket;
    //Texture _interact;
    //Texture _commands;
    //Texture _scanner;
    //SerializedProperty _grappleProp;
    //SerializedProperty _sprocketProp;
    //SerializedProperty _interactProp;
    //SerializedProperty _commandsProp;
    //SerializedProperty _scannerProp;
    //SerializedProperty _playerManagerProp;
    SerializedProperty _componentsProp;
    private void OnEnable()
    {
        //_grapple = Resources.Load<Texture>("Ico_Grapple");
        //_sprocket = Resources.Load<Texture>("Ico_Sprocket");
        //_interact = Resources.Load<Texture>("Ico_Interact");
        //_commands = Resources.Load<Texture>("Ico_Commands");
        //_scanner = Resources.Load<Texture>("Ico_Scanner");
        //_grappleProp = serializedObject.FindProperty("_grapple");
        //_sprocketProp = serializedObject.FindProperty("_sprocket");
        //_interactProp = serializedObject.FindProperty("_interact");
        //_commandsProp = serializedObject.FindProperty("_commands");
        //_scannerProp = serializedObject.FindProperty("_scanner");
        //_playerManagerProp = serializedObject.FindProperty("PlayerManagerSO");
        //_componentsProp = serializedObject.FindProperty("_abilities");
        //if (MonoBehaviourExtension.ISNULL(_componentsProp)) { Debug.Log("NULL BOI"); }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }
        Space(20);
        DrawTitleLayout("Player Manager");
        Space(20);

        // for (int i = 0; i < _componentsProp.arraySize; i++)
        // {
        //     if (i == 0) { Hor(true); FS(); }
        //     else if (i % 2 == 0) { Hor(false); Hor(true); FS(); }
        //     SerializedProperty sp = _componentsProp.GetArrayElementAtIndex(i);
        //     SerializedProperty enabled = sp.FindPropertyRelative("_enabled");
        //     SerializedProperty profile = (sp.FindPropertyRelative("_abilityProfile"));
        //     if (profile != null && profile.objectReferenceValue != null)
        //     {
        //         Texture icon = (profile.objectReferenceValue as System.Object as PlayerAbilityProfile).Icon;


        //         if (icon != null && enabled != null) { DrawPropButton(icon, enabled, 100); continue; }
        //         DrawPropButton(profile.objectReferenceValue.name, enabled, 100, 16);


        //     }
        // }
        // if(_componentsProp.arraySize == 0) { Hor(true); }
        // Hor(false);

        //foreach(Ability a in abilities)
        //{
        //    DrawPropButton(a.Enabled, _grappleProp, size)
        //}

        Space(20);
        DrawAllProperties();
        //Hor(() =>
        //{
        //    FS();
        //    _grappleProp.boolValue = DrawPropButton(_grapple, _grappleProp, size);
        //    _sprocketProp.boolValue = DrawPropButton(_sprocket, _sprocketProp, size);

        //});
        //Hor(() =>
        //{
        //    FS();
        //    _interactProp.boolValue = DrawPropButton(_interact, _interactProp, size);
        //    _commandsProp.boolValue = DrawPropButton(_commands, _commandsProp, size);

        //});
        //Hor(() =>
        //{
        //    FS();
        //    _scannerProp.boolValue = DrawPropButton(_scanner, _scannerProp, size);
        //});

        serializedObject.ApplyModifiedProperties();
    }
    public bool DrawPropButton(Texture icon, SerializedProperty prop, float size = 50)
    {
        GUIStyle style = new(EditorStyles.miniButton) { fixedHeight = size, fixedWidth = size };
        Color hold = GUI.backgroundColor;
        float colorMult = 2.5f;
        if (prop.boolValue) GUI.backgroundColor = new(0.2f * colorMult, 0.8f * colorMult, 1 * colorMult, 1);
        DrawButton(icon, style, () =>
        {
            if (Application.isPlaying) { return; }
            prop.boolValue = !prop.boolValue;
        });
        GUI.backgroundColor = hold;
        return prop.boolValue;
    }
    public bool DrawPropButton(string s, SerializedProperty prop, float size = 50, int _fontSize = 15)
    {
        GUIStyle style = new(EditorStyles.miniButton) { fixedHeight = size, fixedWidth = size, fontSize = _fontSize, fontStyle = FontStyle.Bold, wordWrap = true };
        Color hold = GUI.backgroundColor;
        float colorMult = 2.5f;
        if (prop.boolValue) GUI.backgroundColor = new(0.2f * colorMult, 0.8f * colorMult, 1 * colorMult, 1);
        DrawButton(s, style, () =>
        {
            if (Application.isPlaying) { return; }
            prop.boolValue = !prop.boolValue;
        });
        GUI.backgroundColor = hold;
        return prop.boolValue;
    }
}