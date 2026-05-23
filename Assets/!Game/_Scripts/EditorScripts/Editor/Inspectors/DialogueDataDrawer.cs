using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueData))]
public class DialogueDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Ico_Dialogue.png");
        tex.alphaIsTransparency = true;
        Rect area = new(position.x + (EditorGUIUtility.singleLineHeight * 2), position.y, position.width - (2 * EditorGUIUtility.singleLineHeight), position.height);

        EditorGUI.LabelField(new(position.x, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), new GUIContent(tex));

        DialogueData dData = property.GetUnderlyingValue() as DialogueData;
        var labelName = "Dialogue Data:";
        if (dData != null)
        {
            if (dData.CharacterProfile != null)
            {
                EditorGUI.DrawRect(position, new(dData.CharacterProfile.Color.r, dData.CharacterProfile.Color.g, dData.CharacterProfile.Color.b, 0.1f));
                labelName = dData.CharacterProfile.Name.Replace("<companionname>", CompanionData.CompanionName) + ": ";
            }
            if (dData.TextAsset != null)
            {
                labelName += dData.TextAsset.text.ReplaceAll("\n", " ").RemoveTags();
            }
        }
        EditorGUI.PropertyField(area, property, new GUIContent(labelName), true);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }
    //VisualElement _container;
    //PropertyField _color;
    //PropertyField _persistentDialogueFields;
    //SerializedProperty _property;
    //public override VisualElement CreatePropertyGUI(SerializedProperty property)
    //{
    //    _property = property;
    //    _container = new VisualElement();

    //    _container.Add(new PropertyField(_property.FindPropertyRelative("TextAsset")));
    //    _container.Add(new PropertyField(_property.FindPropertyRelative("StartSFX")));
    //    var propp = new PropertyField(_property.FindPropertyRelative("DiaProfile"));
    //    _container.Add(propp);


    //    propp.RegisterCallback<SerializedPropertyChangeEvent>((v) =>
    //    {
    //        Refresh(v.changedProperty.objectReferenceValue as CharacterProfile);
    //    });

    //    _color = new PropertyField(_property.FindPropertyRelative("_color"));
    //    _container.Add(_color);

    //    _persistentDialogueFields = new PropertyField(_property.FindPropertyRelative("_persistentDialogueFields"));
    //    _container.Add(_persistentDialogueFields);

    //    Refresh(_property.FindPropertyRelative("DiaProfile").objectReferenceValue as CharacterProfile);

    //    return _container;
    //}
    //private void Refresh(CharacterProfile cdp)
    //{
    //    Debug.Log(cdp);
    //    if (cdp == null) { _color.style.display = StyleKeyword.Null; }
    //    else { _color.style.display = DisplayStyle.None; }

    //    if (cdp == null || cdp.Scope != CharacterProfile.Scopes.Persistent)
    //    {
    //        _persistentDialogueFields.style.display = StyleKeyword.Null;
    //    }
    //    else { _persistentDialogueFields.style.display = DisplayStyle.None; }
    //    Debug.Log(_color.style.display);
    //}
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    _textAsset = property.FindPropertyRelative("TextAsset");
    //    _startSFX = property.FindPropertyRelative("StartSFX");
    //    _diaProfile = property.FindPropertyRelative("DiaProfile");
    //    _persistentDiaFields = property.FindPropertyRelative("_persistentDialogueFields");
    //    _color = property.FindPropertyRelative("_color");
    //    var height = 0f;
    //    height +=
    //        EditorGUI.GetPropertyHeight(_textAsset) +
    //        EditorGUI.GetPropertyHeight(_startSFX) +
    //        EditorGUI.GetPropertyHeight(_diaProfile);

    //    _cdp = _diaProfile.objectReferenceValue as CharacterDialogueProfile;

    //    if (_cdp == null)
    //    {
    //        height += EditorGUI.GetPropertyHeight(_color);
    //    }
    //    if (_cdp == null || _cdp.Scope != CharacterDialogueProfile.Scopes.Persistent)
    //    {
    //        height += EditorGUI.GetPropertyHeight(_persistentDiaFields);
    //    }

    //    return height;
    //}
    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //{
    //    var yPos = position.y;

    //    var height = EditorGUI.GetPropertyHeight(_textAsset);
    //    EditorGUI.PropertyField(new(position.x, yPos, position.width, height), _textAsset);
    //    yPos += height;

    //    height = EditorGUI.GetPropertyHeight(_startSFX);
    //    EditorGUI.PropertyField(new(position.x, yPos, position.width, height), _startSFX);
    //    yPos += height;

    //    height = EditorGUI.GetPropertyHeight(_diaProfile);
    //    EditorGUI.PropertyField(new(position.x, yPos, position.width, height), _diaProfile);
    //    yPos += height;

    //    if (_cdp == null)
    //    {
    //        height = EditorGUI.GetPropertyHeight(_color);
    //        EditorGUI.PropertyField(new(position.x, yPos, position.width, height), _color);
    //        yPos += height;
    //    }
    //    if (_cdp == null || _cdp.Scope != CharacterDialogueProfile.Scopes.Persistent)
    //    {
    //        height = EditorGUI.GetPropertyHeight(_persistentDiaFields);
    //        EditorGUI.PropertyField(new(position.x, yPos, position.width, height), _persistentDiaFields, true);
    //        yPos += height;
    //    }
    //}

}
