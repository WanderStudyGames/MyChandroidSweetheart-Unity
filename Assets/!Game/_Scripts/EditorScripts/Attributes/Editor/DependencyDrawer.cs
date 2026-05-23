using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DependencyAttribute))]
public class DependencyDrawer : PropertyDrawer
{
    readonly float lineheight = EditorGUIUtility.singleLineHeight;
    bool isNull;

    private readonly float buttonWidth = 22;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        Rect helpboxRect = new(position.xMin, position.yMin, position.width, lineheight * 2);
        Rect propertyRect = new(position.xMax - ((position.width)), position.yMin, position.width - buttonWidth, lineheight);
        Rect buttonRect = new(position.xMax - buttonWidth, position.yMin, buttonWidth, lineheight);

        isNull = property.objectReferenceValue == null;

        //if (Event.current.type == EventType.Layout)
        //{
        //EditorGUILayout.BeginHorizontal();
        //}
        if (isNull)
        {
            EditorGUI.HelpBox(helpboxRect, "Unassigned Reference", MessageType.Error);
            propertyRect.y += helpboxRect.height;
            buttonRect.y += helpboxRect.height;
            //Debug.LogError($"'{property.serializedObject.targetObject.name}.{property.name}' is null!");
        }

        GUI.enabled = isNull;


        EditorGUI.PropertyField(propertyRect, property, new GUIContent(ObjectNames.NicifyVariableName(property.name)));

        GUI.enabled = true;

        if (!isNull && GUI.Button(buttonRect, "X"))
        {
            property.objectReferenceValue = null;
        }
        //if (Event.current.type == EventType.Layout)
        //{
        //EditorGUILayout.EndHorizontal();
        //}


    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lines = 1;
        if (isNull) { lines += 3; }

        return lines * lineheight;
    }
}
