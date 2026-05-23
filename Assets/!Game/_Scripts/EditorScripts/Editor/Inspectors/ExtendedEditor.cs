using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ExtendedEditor : Editor
{
    public static bool Layout => (Event.current.type == EventType.Layout);
    public readonly static float singleLineHeight = EditorGUIUtility.singleLineHeight;
    public static Rect GetRect() { return GUILayoutUtility.GetRect(new GUIContent(), new GUIStyle()); }
    public static void Hor(bool b) { if (b) EditorGUILayout.BeginHorizontal(); else EditorGUILayout.EndHorizontal(); }
    public static void Hor(Action a)
    {
        EditorGUILayout.BeginHorizontal();
        a();
        EditorGUILayout.EndHorizontal();
    }
    public static void Ver(bool b) { if (b) EditorGUILayout.BeginVertical(); else EditorGUILayout.EndVertical(); }
    public static void Ver(Action a)
    {
        EditorGUILayout.BeginVertical();
        a();
        EditorGUILayout.EndVertical();
    }
    public static void Space(float amt) { EditorGUILayout.Space(amt); }
    public static void FS() { GUILayout.FlexibleSpace(); }
    public bool DrawProperty(string name)
    {
        return DrawProperty(name, serializedObject);
    }
    public static bool DrawProperty(SerializedProperty property)
    {
        return EditorGUILayout.PropertyField(property);
    }
    public static bool DrawProperty(string name, SerializedObject so)
    {
        if (so.FindProperty(name) == null) return false;
        return EditorGUILayout.PropertyField(so.FindProperty(name));
    }
    public bool DrawProperty(string name, GUILayoutOption layoutOption)
    {
        if (serializedObject.FindProperty(name) == null) return false;
        return EditorGUILayout.PropertyField(serializedObject.FindProperty(name), layoutOption);
    }

    public static bool DrawLayoutFoldoutGroup(bool foldout, string label, Action display)
    {
        foldout = EditorGUILayout.Foldout(foldout, new GUIContent(label));
        if (foldout)
        {
            EditorGUI.indentLevel++;
            display();
            EditorGUI.indentLevel--;

        }
        return foldout;
    }

    public static void DrawButton(string label, Action action)
    {
        if (GUILayout.Button(new GUIContent(label), EditorStyles.miniButton))
        {
            action();
        }
    }
    public static void DrawButton(string label, GUIStyle style, Action action)
    {
        if (GUILayout.Button(new GUIContent(label), style))
        {
            action();
        }
    }
    public static void DrawButton(Texture texture, GUIStyle style, Action action)
    {
        if (GUILayout.Button(texture, style))
        {
            action();
        }
    }
    public static void DrawButton(Texture texture, string label, GUIStyle style, Action action)
    {
        if (GUILayout.Button(new GUIContent(label, texture), style))
        {
            action();
        }
    }
    public static void DrawTextureLabel(Texture texture, float width = 40, float height = 30)
    {
        GUILayout.Label(texture, GUILayout.Height(height), GUILayout.Width(width));
    }
    public static void DrawHeader(string text)
    {
        GUIStyle style = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField(text, style);
    }
    public static void DrawSeparator(float height = 2, float spacing = 10, float offset = 0)
    {
        Rect position = ExtendedEditor.GetRect();
        Rect r = new(new(position.xMin, position.yMin + spacing + offset), new(position.width, height));
        EditorGUI.DrawRect(r, new(0.28f, 0.28f, 0.28f));
        //EditorGUILayout.Space((spacing * 2) + height);
    }
    public static void DrawTitleLayout(string title, float height = 20f, int _fontSize = 20)
    {
        GUIStyle titleStyle = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = _fontSize };
        EditorGUILayout.LabelField(title, titleStyle, GUILayout.Height(height));
    }
    public static void DrawTitle(Rect position, string title, int _fontSize = 20)
    {
        GUIStyle titleStyle = new(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = _fontSize };
        EditorGUI.LabelField(position, title, titleStyle);
    }
    public void DrawProperties(SerializedProperty iterator)
    {
        iterator.NextVisible(true);
        while (iterator.NextVisible(false))
        {
            DrawProperty(iterator.name);
        }
    }

    public bool DrawDependency(string name, GUILayoutOption layoutOption)
    {
        return DrawDependency(name, serializedObject, layoutOption);
    }

    public bool DrawDependency(string name)
    {
        return DrawDependency(name, serializedObject);
    }

    public static bool DrawDependency(string name, SerializedObject serializedObject, GUILayoutOption layoutOption)
    {
        var prop = serializedObject.FindProperty(name);
        if (prop.IsUnityNull() || prop.objectReferenceValue != null) return false;
        return EditorGUILayout.PropertyField(prop, layoutOption);
    }
    public static bool DrawDependency(string name, SerializedObject serializedObject)
    {
        var prop = serializedObject.FindProperty(name);
        if (prop.IsUnityNull() || prop.objectReferenceValue != null) return false;
        return EditorGUILayout.PropertyField(prop);
    }

    public void DrawScript()
    {
        DrawScript(serializedObject);
    }
    public static void DrawScript(SerializedObject serializedObject)
    {
        GUI.enabled = false;
        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
        GUI.enabled = true;
    }
    public void DrawAllProperties()
    {
        DrawScript();
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);
        while (iterator.NextVisible(false))
        {
            DrawProperty(iterator.name);
        }
    }
    public void DrawTitleBlock(Texture _icon, string title)
    {
        Hor(() =>
        {
            DrawTextureLabel(_icon, 50, 50);

            Ver(() =>
            {
                FS();
                DrawTitleLayout(title);
                Hor(true);
                FS();
                DrawHeader(serializedObject.FindProperty("m_Name").stringValue);
                FS();
                Hor(false);
                FS();
            });

        });
    }
}
