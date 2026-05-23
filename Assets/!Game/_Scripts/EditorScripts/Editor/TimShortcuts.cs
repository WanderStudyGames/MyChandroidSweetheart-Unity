using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class TimShortcuts
{
    [MenuItem("GameObject/Select Prefab")]
    public static void SelectPrefab()
    {
        if (Selection.activeObject == null) return;
        var obj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(Selection.activeObject);
        var path = AssetDatabase.GetAssetPath(obj);
        if (path != "")
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }
    }
    [MenuItem("GameObject/Send To Bottom")]
    public static void SendToBottom()
    {
        if (Selection.objects.Length == 0) return;
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            var obj = Selection.objects[i] as GameObject;
            if (obj == null) continue;
            obj.transform.SetAsLastSibling();
        }
    }
    [MenuItem("GameObject/Send To Top")]
    public static void SendToTop()
    {
        if (Selection.objects.Length == 0) return;
        for (int i = Selection.objects.Length - 1; i >= 0; i--)
        {
            var obj = Selection.objects[i] as GameObject;
            if (obj == null) continue;
            obj.transform.SetAsFirstSibling();
        }
    }
    [MenuItem("Select/Event State Machine")]
    public static void SelectEventStateMachine()
    {
        SelectByType<EventStateMachine>();
    }
    [MenuItem("Select/Timeline Playable Director")]
    public static void SelectPlayableDirector()
    {
        SelectByType<PlayableDirector>();
    }
    [MenuItem("Select/Power Fork")]
    public static void SelectPowerFork()
    {
        SelectByType<PowerStateMachineController>();
    }
    [MenuItem("Select/Lightmap Switcher")]
    public static void SelectLightMapSwitcher()
    {
        SelectByType<LightMapSwitcher>();
    }
    [MenuItem("Select/Nav Mesh Surface")]
    public static void SelectNavMeshSurface()
    {
        var surface = Object.FindObjectOfType(typeof(NavMeshSurface)) as NavMeshSurface;
        if (surface == null) return;
        surface.enabled = false;
        surface.enabled = true;
        Selection.activeGameObject = surface.gameObject;
    }
    [MenuItem("Select/SceneStartup")]
    public static void SelectSceneStartup()
    {
        SelectByType<SceneStartup>();
    }
    private static void SelectByType<T>(bool openProperties = false) where T : Object
    {
        var obj = Object.FindObjectOfType(typeof(T));
        Selection.activeObject = obj;
        if (openProperties && obj != null)
            EditorUtility.OpenPropertyEditor(obj);
    }

}