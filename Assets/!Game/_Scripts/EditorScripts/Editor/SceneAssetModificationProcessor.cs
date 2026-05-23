using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneAssetModificationProcessor
{
    [InitializeOnLoadMethod]
    public static void Subscribe()
    {
        EditorSceneManager.sceneSaved += BakeNavMesh;
    }

    [MenuItem("NavMesh/Bake Nav Mesh")]
    public static void BakeNavMesh()
    {
        BakeNavMesh(SceneManager.GetActiveScene());
    }
    public static void BakeNavMesh(Scene scene)
    {
        NavMeshSurface[] surfaces = GameObject.FindObjectsOfType<NavMeshSurface>();
        foreach (NavMeshSurface surface in surfaces)
        {
            string newPath = scene.path.Replace(".unity", $"/NavMesh-{surface.gameObject.name}.asset");
            if (string.IsNullOrEmpty(newPath))
            {
                Debug.LogError($"Scene path is null or empty for scene {scene.name}");
                continue;
            }
            surface.RemoveData();
            surface.BuildNavMesh();

            //NavMeshBuilder.BuildNavMeshData(surface.GetBuildSettings(), new List<NavMeshBuildSource>(), new Bounds(), surface.transform.position, Quaternion.Euler(Vector3.up));
            if (surface.navMeshData == null) { Debug.LogError($"NavMesh Data is null for surface on {surface.gameObject.name}"); continue; }
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(newPath)))
                AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(newPath.AscendDir()), System.IO.Path.GetFileNameWithoutExtension(newPath.AscendDir()));
            AssetDatabase.CreateAsset(surface.navMeshData, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            surface.navMeshData = AssetDatabase.LoadAssetAtPath<NavMeshData>(newPath);
            EditorUtility.SetDirty(surface);

            //refresh the navmesh by toggling enabled state
            surface.enabled = false;
            surface.enabled = true;
        }
    }
}