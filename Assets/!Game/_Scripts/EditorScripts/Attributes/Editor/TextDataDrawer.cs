using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomPropertyDrawer(typeof(TextDataAttribute))]
public class TextDataDrawer : PropertyDrawer
{
    string _placeholderText;
    SerializedProperty _textAsset;
    const string _pathSaveBase = "!Game/Text/Resources/";
    private TextDataAttribute _attribute;
    private static string _path = "";
    string _assetName = "";
    string _fileName;
    public Texture _icon;
    bool _assetExists = false;
    private bool _initialized;
    private float _padding = 3f;
    private void Refresh()
    {
        AssetDatabase.Refresh();
        _placeholderText = "";
        _attribute = (TextDataAttribute)attribute;
        if (_textAsset != null && _textAsset.objectReferenceValue != null)
        {
            _assetExists = true;
            _placeholderText = _textAsset.objectReferenceValue.ToString();
            _assetName = _textAsset.objectReferenceValue.name;
        }
        _fileName = _attribute.NameStarter;
        if (SceneManager.GetActiveScene() != null) _fileName = _fileName + SceneManager.GetActiveScene().name + "_";
        if (_assetExists) _fileName = _textAsset.objectReferenceValue.name;

    }
    private void GenerateFile()
    {
        string filePath = Application.dataPath + "/" + _pathSaveBase + _attribute.PathRoot + _path;
        if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
        System.IO.File.WriteAllText(filePath + "/" + _fileName + ".txt", _placeholderText);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight * 7 + (_padding * 8);
        }
        else
        {
            return EditorGUIUtility.singleLineHeight + (_padding * 4);
        }
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var yPos = position.y + _padding * 2;
        _textAsset = property;
        EditorGUI.DrawRect(new(position.x, position.y, position.width, GetPropertyHeight(property, label)), new(0f, 0f, 0f, 0.1f));
        if (!_initialized) { Refresh(); _initialized = true; }

        //check for manual changes to textasset
        if (_assetExists && _textAsset.objectReferenceValue == null) { Refresh(); }
        if (!_assetExists && property.objectReferenceValue != null) { Refresh(); }
        if (_assetExists && _textAsset.objectReferenceValue.name != _assetName) { Refresh(); }

        property.isExpanded = EditorGUI.Foldout(new(position.x, yPos, position.width * 0.5f, EditorGUIUtility.singleLineHeight), property.isExpanded, new GUIContent());

        EditorGUI.PropertyField(new(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight), property);
        yPos += EditorGUIUtility.singleLineHeight + _padding;

        if (!property.isExpanded) return;

        GUIStyle textAreaStyle = new(EditorStyles.textArea) { wordWrap = true, stretchHeight = false };
        _placeholderText = EditorGUI.TextArea(new(position.x, yPos, position.width, ExtendedEditor.singleLineHeight * 3), _placeholderText, textAreaStyle);
        yPos += ExtendedEditor.singleLineHeight * 3 + _padding;

        GUIStyle labelStyle = new(EditorStyles.label) { alignment = TextAnchor.MiddleRight };
        EditorGUI.LabelField(new(position.x, yPos, position.width, EditorGUIUtility.singleLineHeight), _pathSaveBase + _attribute.PathRoot, labelStyle);
        yPos += ExtendedEditor.singleLineHeight + _padding;


        EditorGUI.LabelField(new(position.x, yPos, position.width * .25f, EditorGUIUtility.singleLineHeight), "File Name", labelStyle);
        _fileName = EditorGUI.TextField(new(position.x + (position.width * .25f), yPos, position.width * .75f, EditorGUIUtility.singleLineHeight), _fileName);
        yPos += ExtendedEditor.singleLineHeight + _padding;


        GUIStyle buttonStyle = new(EditorStyles.miniButton) { alignment = TextAnchor.MiddleCenter };
        if (GUI.Button(new(position.x + (position.width * .25f), yPos, position.width / 2, EditorGUIUtility.singleLineHeight), new GUIContent("Write To File & Assign"), buttonStyle))
        {
            GenerateFile();
            AssetDatabase.Refresh();
            _textAsset.objectReferenceValue = Resources.Load<TextAsset>(_attribute.PathRoot + _path + _fileName);
        }
    }
}