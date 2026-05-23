//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//[CustomEditor(typeof(Scan))]
//public class ScanEditor : ExtendedEditor
//{
//    string _placeholderText;
//    SerializedProperty _textAsset;
//    const string _pathSaveBase = "!Game/Text/Resources/ScanTexts/";
//    const string _pathLookupBase = "ScanTexts/";
//    static string _path = "";
//    string _assetName = "";
//    string _fileName;
//    Texture _icon;
//    bool _assetExists = false;
//    static bool _displayAdditionalGO = false;
//    private void OnEnable()
//    {
//        Refresh();
//        _icon = Resources.Load<Texture2D>("Icons/Ico_Scan");



//    }
//    private void Refresh()
//    {
//        AssetDatabase.Refresh();
//        Repaint();
//        _assetExists = false;
//        serializedObject.Update();
//        _textAsset = serializedObject.FindProperty("textAsset");
//        _placeholderText = "";
//        if (_textAsset.objectReferenceValue != null)
//        {
//            _assetExists = true;
//            _placeholderText = _textAsset.objectReferenceValue.ToString();
//            _assetName = _textAsset.objectReferenceValue.name;
//        }
//        serializedObject.ApplyModifiedProperties();

//        _fileName = "Scan_";
//        if (SceneManager.GetActiveScene() != null) _fileName = "Scan_" + SceneManager.GetActiveScene().name + "_";
//        if (_assetExists) _fileName = _textAsset.objectReferenceValue.name;
//    }
//    public override void OnInspectorGUI()
//    {
//        //check for manual changes to textasset
//        if (_assetExists && _textAsset.objectReferenceValue == null) { Refresh(); }
//        if (!_assetExists && serializedObject.FindProperty("textAsset").objectReferenceValue != null) { Refresh(); }
//        if (_assetExists && _textAsset.objectReferenceValue.name != _assetName) { Refresh(); }


//        serializedObject.Update();

//        GUIStyle titleStyle = new(EditorStyles.boldLabel) { alignment = TextAnchor.UpperCenter, fontSize = 20 };
//        EditorGUILayout.LabelField("Scan Editor", titleStyle, GUILayout.Height(20));
//        Space(10);
//        if (serializedObject.FindProperty("scanMaterialsSO").objectReferenceValue == null)
//        {
//            DrawProperty("scanMaterialsSO");
//        }
//        DrawProperty("onScannerEnableGE");
//        DrawProperty("onScannerDisableGE");
//        DrawProperty("OnLeaveScanGE");
//        DrawScript();
//        DrawProperty("textAsset");
//        Space(10);

//        Hor(true);
//        DrawTextureLabel(_icon);
//        GUIStyle textAreaStyle = new(EditorStyles.textArea) { wordWrap = true, stretchHeight = true };
//        _placeholderText = EditorGUILayout.TextArea(_placeholderText, textAreaStyle);
//        Hor(false);

//        Hor(true);
//        GUIStyle labelStyle = new(EditorStyles.label) { alignment = TextAnchor.MiddleRight, stretchWidth = true };
//        EditorGUILayout.LabelField(_pathSaveBase + _pathLookupBase, labelStyle);
//        GUIStyle pathstyle = new(EditorStyles.textField) { stretchWidth = true, stretchHeight = true, wordWrap = true, alignment = TextAnchor.MiddleLeft };
//        _path = EditorGUILayout.TextArea(_path, pathstyle);
//        Hor(false);

//        Hor(true);
//        EditorGUILayout.LabelField("File Name", GUILayout.Width(100));
//        _fileName = EditorGUILayout.TextField(_fileName);
//        Hor(false);



//        Hor(true);
//        GUILayout.FlexibleSpace();
//        GUIStyle buttonStyle = new(EditorStyles.miniButton) { fixedWidth = 200 };
//        DrawButton("Write To File & Assign", buttonStyle, () =>
//        {
//            GenerateScanFile();
//            AssetDatabase.Refresh();
//            _textAsset.objectReferenceValue = Resources.Load<TextAsset>(_pathLookupBase + _path + _fileName);
//            Repaint();
//        });
//        Hor(false);

//        DrawSeparator(2, 0, 7);

//        Space(10);

//        DrawProperty("important");
//        DrawProperty("includeAllChildren");
//        DrawProperty("includeSelf");
//        DrawProperty("scanCompletedAction");
//        DrawProperty("performOnce");
//        Space(10);

//        _displayAdditionalGO = DrawLayoutFoldoutGroup(_displayAdditionalGO, "Additional Holograms", () =>
//        {
//            DrawProperty("gameObjects");
//        });




//        Space(singleLineHeight);
//        serializedObject.ApplyModifiedProperties();
//    }

//    private void GenerateScanFile()
//    {
//        string filePath = Application.dataPath + "/" + _pathSaveBase + _path;
//        if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
//        System.IO.File.WriteAllText(filePath + "/" + _fileName + ".txt", _placeholderText);
//    }
//}
