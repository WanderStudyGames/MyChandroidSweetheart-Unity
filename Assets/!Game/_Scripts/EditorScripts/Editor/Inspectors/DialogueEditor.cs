using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Dialogue))]

public class DialogueEditor : ExtendedEditor
{
    string _placeholderText;
    SerializedProperty _textAsset;
    SerializedProperty _freezePlayer;
    SerializedProperty _cdp;
    const string _pathBase = "!Game/Text/Resources/DialogueTexts/";
    const string _loadPath = "DialogueTexts/";
    static string _path = "";
    string _assetName = "";
    string _fileName;
    Texture _icon;
    bool _assetExists = false;
    //static bool _displayAdditionalGO = false;
    private void OnEnable()
    {
        Refresh();
        _icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Ico_Dialogue.png");



    }
    private void Refresh()
    {
        AssetDatabase.Refresh();
        Repaint();
        _assetExists = false;
        serializedObject.Update();
        _textAsset = serializedObject.FindProperty("textAsset");
        _cdp = serializedObject.FindProperty("characterDialogueProfile");
        _placeholderText = "";
        if (_textAsset.objectReferenceValue != null)
        {
            _assetExists = true;
            _placeholderText = _textAsset.objectReferenceValue.ToString();
            _assetName = _textAsset.objectReferenceValue.name;
        }
        serializedObject.ApplyModifiedProperties();

        _fileName = "Dialogue_";
        if (SceneManager.GetActiveScene() != null) _fileName += SceneManager.GetActiveScene().name + "_";
        if (_assetExists) _fileName = _textAsset.objectReferenceValue.name;
    }
    public override void OnInspectorGUI()
    {
        //check for manual changes to textasset
        if (_assetExists && _textAsset.objectReferenceValue == null) { Refresh(); }
        if (!_assetExists && serializedObject.FindProperty("textAsset").objectReferenceValue != null) { Refresh(); }
        if (_assetExists && _textAsset.objectReferenceValue.name != _assetName) { Refresh(); }


        serializedObject.Update();



        GUIStyle titleStyle = new(EditorStyles.boldLabel) { alignment = TextAnchor.UpperCenter, fontSize = 20 };
        EditorGUILayout.LabelField("Dialogue Editor", titleStyle, GUILayout.Height(20));
        Space(10);

        DrawScript();
        DrawSeparator(2, 0, 7);
        //if (serializedObject.FindProperty("scanMaterialsSO").objectReferenceValue == null)
        //{
        //    DrawProperty("scanMaterialsSO");
        //}
        //DrawScript();
        DrawProperty("textAsset");
        Space(10);

        Hor(true);
        DrawTextureLabel(_icon);
        GUIStyle textAreaStyle = new(EditorStyles.textArea) { wordWrap = true, stretchHeight = true };
        _placeholderText = EditorGUILayout.TextArea(_placeholderText, textAreaStyle);
        Hor(false);

        Hor(true);
        GUIStyle labelStyle = new(EditorStyles.label) { alignment = TextAnchor.MiddleRight, stretchWidth = true };
        EditorGUILayout.LabelField(_pathBase, labelStyle);
        GUIStyle pathstyle = new(EditorStyles.textField) { stretchWidth = true, stretchHeight = true, wordWrap = true, alignment = TextAnchor.MiddleLeft };
        _path = EditorGUILayout.TextArea(_path, pathstyle);
        Hor(false);

        Hor(true);
        EditorGUILayout.LabelField("File Name", GUILayout.Width(100));
        _fileName = EditorGUILayout.TextField(_fileName);
        Hor(false);



        Hor(true);
        GUILayout.FlexibleSpace();
        GUIStyle buttonStyle = new(EditorStyles.miniButton) { fixedWidth = 200 };
        DrawButton("Write To File & Assign", buttonStyle, () =>
        {
            GenerateDialogueFile();
            AssetDatabase.Refresh();
            string pathName = _path + "/";
            if (_path == "") pathName = "";
            _textAsset.objectReferenceValue = Resources.Load<TextAsset>(_loadPath + pathName + _fileName);
            Repaint();
        });
        Hor(false);

        DrawSeparator(2, 0, 7);

        DrawProperty("nextDialogue");
        DrawSeparator(2, 0, 7);

        DrawProperty("dialogueSFX");
        DrawSeparator(2, 0, 7);
        DrawProperty("characterDialogueProfile");
        DrawProperty("diaCamera");
        DrawProperty("focusObject");
        DrawProperty("freezePlayerLerpSpeed");
        DrawProperty("color");
        DrawSeparator(2, 0, 7);
        DrawProperty("executeOnFinish");

        serializedObject.ApplyModifiedProperties();
    }

    private void GenerateDialogueFile()
    {
        string filePath = Application.dataPath + "/" + _pathBase + _path;
        if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
        System.IO.File.WriteAllText(filePath + "/" + _fileName + ".txt", _placeholderText);
    }
}