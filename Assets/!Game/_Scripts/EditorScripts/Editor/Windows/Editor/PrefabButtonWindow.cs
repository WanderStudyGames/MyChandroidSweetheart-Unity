using UnityEditor;
using UnityEngine;

[EditorWindowTitle(title = "Scenes")]
public class PrefabButtonWindow : AssetButtonWindow<Object>
{
    bool _showAll = false;
    float _size = 60f;
    protected override string Query => "t:Prefab " + query;
    protected override string startFolder => _showAll ? base.startFolder : base.startFolder + "/_Prefabs/Props";
    [MenuItem("Window/Prefab Viewer")]
    public static void Enable()
    {
        EditorWindow.GetWindow<PrefabButtonWindow>("Props");
    }
    protected override void OnGUI()
    {
        ExtendedEditor.Hor(true);
        _showAll = EditorGUILayout.ToggleLeft("Show All Prefabs", _showAll);
        _size = EditorGUILayout.Slider(_size, 50, 100);
        ExtendedEditor.Hor(false);
        base.OnGUI();
    }
    protected override void DrawAssets(Object[] objects)
    {
        var maxWidth = EditorGUIUtility.currentViewWidth;
        var j = 0f;
        ExtendedEditor.Hor(true);
        foreach (Object obj in objects)
        {
            if (j + _size > maxWidth) { ExtendedEditor.Hor(false); ExtendedEditor.Hor(true); j = 0; }
            DrawAsset(obj);
            j += _size;
        }
        ExtendedEditor.Hor(false);
        ExtendedEditor.Space(60f);
    }
    protected override void DrawAsset(Object obj)
    {
        var style = new GUIStyle(EditorStyles.iconButton)
        {
            fixedHeight = _size,
            fixedWidth = _size,
            margin = new RectOffset(0, 0, 0, 0),
            padding = new(4, 4, 4, 4)
        };
        if (GUILayout.Button(new GUIContent(AssetPreview.GetAssetPreview(obj)), style))
        {
            Open(obj);
        }
    }
    protected override void Open(Object obj)
    {
        var prefab = PrefabUtility.InstantiatePrefab(obj) as GameObject;
        if (prefab != null)
        {
            Selection.objects = null;
            Selection.activeGameObject = prefab;
            SceneView.lastActiveSceneView.MoveToView();
            EditorUtility.SetDirty(prefab);
        }

    }
}
