using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class ContextMenuShortcuts
{
    [MenuItem("Assets/Create SFX", isValidateFunction: true)]
    [MenuItem("Assets/Create SFX Individually", isValidateFunction: true)]
    public static bool IsAudioFile()
    {
        bool b = false;
        foreach (var obj in Selection.objects) { if (obj.GetType() == typeof(AudioClip)) b = true; }
        return b;
    }
    [MenuItem("Assets/Generate Texture From Cubemap", isValidateFunction: true)]
    public static bool IsCubemap() { return Selection.activeObject is Cubemap; }
    [MenuItem("Assets/Add SFX To Scene", isValidateFunction: true)]
    public static bool IsSFX() { if (Selection.activeObject == null) return false; return Selection.activeObject.GetType() == typeof(SFX); }
    [MenuItem("Assets/Create Scene Profile", isValidateFunction: true)]
    public static bool IsScene() { return Selection.activeObject.GetType() == typeof(SceneAsset); }

    [MenuItem("Assets/Create SFX")]
    public static void Create()
    {
        List<AudioClip> clips = new();
        Object[] selections = Selection.objects;

        for (int i = selections.Length - 1; i >= 0; i--)
        {
            AudioClip clip = selections[i] as AudioClip;
            if (clip != null)
            {
                clips.Add((AudioClip)selections[i]);

            }
        }
        if (clips.Count > 0)
        {
            CreateSFX(clips.ToArray());
        }
        Selection.objects = null;
    }
    [MenuItem("Assets/Create SFX Individually")]
    public static void CreateIndividual()
    {
        Object[] selections = Selection.objects;

        for (int i = selections.Length - 1; i >= 0; i--)
        {
            AudioClip clip = selections[i] as AudioClip;
            if (clip != null)
            {

                CreateSFX(new AudioClip[] { clip });
            }
        }
        Selection.objects = null;
    }
    private static void CreateSFX(AudioClip[] clips)
    {
        SFX sfx = Editor.CreateInstance<SFX>();
        sfx.Init(clips);
        string path = AssetDatabase.GetAssetPath(clips[0]);
        path = path.Split(clips[0].name)[0];
        string filename = clips[0].name.Replace("au_", "");
        if (AssetDatabase.LoadAssetAtPath<SFX>(path + $"SFX_{filename}.asset") == null)
        {
            AssetDatabase.CreateAsset(sfx, path + $"SFX_{filename}.asset");
        }
        else
        {
            int i = 0;
            while (AssetDatabase.LoadAssetAtPath<SFX>(path + $"SFX_{filename}_{i}.asset") != null)
            {
                i++;
            }
            AssetDatabase.CreateAsset(sfx, path + $"SFX_{filename}_{i}.asset");
        }
        Selection.objects = null;
    }
    [MenuItem("Assets/Add SFX To Scene")]
    public static void AddSFXToScene()
    {
        var sfxAsset = Selection.activeObject as SFX;
        if (sfxAsset == null) return;
        var go = new GameObject(sfxAsset.name);
        var sfxsrc = go.AddComponent<SFXSource>();
        sfxsrc.SetSFX(sfxAsset);
        sfxsrc.playOnAwake = true;
        SceneView.lastActiveSceneView.MoveToView(go.transform);
        Selection.activeGameObject = go;
    }
    [MenuItem("GameObject/Unwalkable")]
    private static void Unwalkable()
    {
        var objs = Selection.gameObjects;
        if (objs.Length == 0) return;
        foreach (var obj in objs)
        {
            Undo.RecordObject(obj, obj.name + ": Add NavMeshModifier");
            var mod = obj.AddComponent<NavMeshModifier>();
            mod.overrideArea = true;
            mod.area = 1;
            obj.layer = LayerMask.NameToLayer("env");
            if (obj.transform.parent != null) obj.transform.parent.gameObject.layer = LayerMask.NameToLayer("env");
            EditorUtility.SetDirty(obj);
        }
        Selection.objects = null;

    }
    [MenuItem("GameObject/Setup Prefab")]
    private static void SetupPrefab()
    {
        var objs = Selection.gameObjects;
        var aObj = Selection.activeGameObject;
        if (objs.Length == 0) return;
        if (aObj == null) return;
        var name = "Pf_" + aObj.name.FirstCharacterToUpper();
        var parent = new GameObject(name);
        parent.transform.position = aObj.transform.position;
        foreach (var obj in objs)
        {
            obj.transform.SetParent(parent.transform, true);
            EditorUtility.SetDirty(obj);
        }
        EditorUtility.SetDirty(parent);
        //must set selection as null because function runs once for every game object in selection
        Selection.objects = null;
    }
    [MenuItem("Assets/Generate Prefab(s)")]
    private static void GenerateWalkablePrefab()
    {
        foreach (var obj in Selection.gameObjects)
        {
            GeneratePrefab(obj);
        }
    }
    [MenuItem("Assets/Generate Unwalkable Prefab(s)")]
    private static void GenerateUnWalkablePrefab()
    {
        foreach (var obj in Selection.gameObjects)
        {
            GeneratePrefab(obj, true);
        }
    }
    private static void GeneratePrefab(GameObject obj, bool unwalkable = false)
    {
        if (obj == null) return;
        string path = AssetDatabase.GetAssetPath(obj);
        string directory = path[..path.LastIndexOf('/')];
        string name = "Pf_" + obj.name.FirstCharacterToUpper();
        var parent = new GameObject(name);

        var childMesh = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        if (childMesh == null) return;

        var child = PrefabUtility.InstantiatePrefab(childMesh, parent.transform) as GameObject;
        if (unwalkable)
        {
            var mod = child.AddComponent<NavMeshModifier>();
            mod.overrideArea = true;
            mod.area = 1;
            child.layer = LayerMask.NameToLayer("env");
            parent.layer = LayerMask.NameToLayer("env");
        }
        PrefabUtility.SaveAsPrefabAsset(parent, directory + "/" + name + ".prefab");
        GameObject.DestroyImmediate(parent);

        Selection.objects = null;
    }
    [MenuItem("GameObject/Collider/Box Collider", priority = 10)]
    private static void BoxCollider()
    {
        foreach (var obj in Selection.gameObjects)
        {
            obj.AddComponent<BoxCollider>();
            EditorUtility.SetDirty(obj);
        }
        Selection.objects = null;
    }
    [MenuItem("GameObject/Collider/Mesh Collider", priority = 10)]
    private static void MeshCollider()
    {
        foreach (var obj in Selection.gameObjects)
        {
            obj.AddComponent<MeshCollider>();
            EditorUtility.SetDirty(obj);
        }
        Selection.objects = null;
    }
    [MenuItem("GameObject/Copy Active Object's Transform", priority = 10)]
    private static void CopyActiveTransform()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (obj != Selection.activeGameObject)
            {
                Undo.RecordObject(obj.transform, "Copy Active Object's Transform");
                obj.transform.SetPositionAndRotation(Selection.activeGameObject.transform.position, Selection.activeGameObject.transform.rotation);
                obj.transform.localScale = Selection.activeGameObject.transform.localScale;
                if (obj.transform is RectTransform && Selection.activeGameObject.transform is RectTransform)
                {
                    var objt = obj.transform as RectTransform;
                    var actT = Selection.activeGameObject.transform as RectTransform;
                    objt.sizeDelta = actT.sizeDelta;
                    objt.anchorMin = actT.anchorMin;
                    objt.anchorMax = actT.anchorMax;
                    objt.pivot = actT.pivot;
                }
            }

        }
        Selection.objects = null;
    }
    [MenuItem("Assets/Generate Texture From Cubemap")]
    private static void CreateTextureFromCubemap()
    {
        if (Selection.activeObject is not Cubemap) return;

        Cubemap cubemap = (Cubemap)Selection.activeObject;


        TextureFromCubeMapPopup.Open(cubemap);

        Selection.activeObject = null;
    }
    [MenuItem("GameObject/3D Object/Wire")]
    private static void CreateBlackLine()
    {
        var lineRenderer = new GameObject("Wire").AddComponent<LineRenderer>();
        var matPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("WireAnim")[0]);

        lineRenderer.material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.positionCount = 0;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.useWorldSpace = false;
        Selection.objects = null;
        Selection.activeGameObject = lineRenderer.gameObject;
        SceneView.lastActiveSceneView.MoveToView();
    }
    private static string _uniqueVersionPath = string.Empty;
    [MenuItem("Assets/Copy To Folder")]
    private static void MakeUniqueVersion()
    {
        var startDir = _uniqueVersionPath;
        if (startDir == string.Empty) startDir = AssetDatabase.GetAssetPath(Selection.activeObject).AscendDir();
        var path = EditorUtility.SaveFolderPanel($"Save new {Selection.activeObject.GetType().Name}", startDir, "");
        _uniqueVersionPath = path;
        foreach (Object activeObject in Selection.objects)
        {
            var objectPath = AssetDatabase.GetAssetPath(activeObject);
            var ext = objectPath.Substring(objectPath.LastIndexOf('.') + 1);
            var newObjectPath = path + $"/{activeObject.name}_.{ext}";
            AssetDatabase.CopyAsset(objectPath, newObjectPath);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(newObjectPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
