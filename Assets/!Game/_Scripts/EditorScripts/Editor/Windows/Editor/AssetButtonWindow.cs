using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AssetButtonWindow<T> : EditorWindow where T : Object
{
    protected string query = "";
    protected virtual string Query => query;
    protected virtual string startFolder => "Assets/!Game";

    readonly List<Object> _assets = new();
    protected Vector2 scrollPos = new();

    private void GetAssets()
    {
        _assets.Clear();
        string[] assetGUIDs = AssetDatabase.FindAssets(Query, new string[] { startFolder });
        foreach (string s in assetGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(s);
            var obj = AssetDatabase.LoadAssetAtPath<T>(path);
            if (obj != null && obj.name != "GameScene")
            {
                _assets.Add(obj);
            }

        }
    }
    protected void OnEnable()
    {
        GetAssets();
    }
    protected void OnHierarchyChange()
    {
        GetAssets();
    }

    protected virtual void OnGUI()
    {
        GetAssets();
        query = EditorGUILayout.TextField(query);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height));

        DrawAssets(_assets.ToArray());

        EditorGUILayout.EndScrollView();
        ExtendedEditor.Space(30);
    }
    protected virtual void DrawAssets(Object[] objects)
    {
        foreach (Object obj in objects)
        {
            DrawAsset(obj);
        }
        ExtendedEditor.Space(60);
    }
    protected virtual void DrawAsset(Object obj)
    {
        var style = new GUIStyle(EditorStyles.miniButton);
        style.alignment = TextAnchor.MiddleLeft;
        style.fixedWidth = position.width - 20;
        ExtendedEditor.Hor(() =>
        {
            ExtendedEditor.DrawButton(AssetPreview.GetMiniThumbnail(obj), obj.name, style, () =>
            {
                ClickFunction(obj);
            });
            ExtendedEditor.FS();
        });
    }
    protected virtual void ClickFunction(Object obj)
    {
        if (Selection.activeObject == obj)
        {
            Open(obj);
        }
        else
        {
            Selection.activeObject = obj;
        }
    }
    protected virtual void Open(Object obj)
    {
        AssetDatabase.OpenAsset(obj);
    }
}
