using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VInspector;

public class BakeLineRendererToMesh : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers; // Updated to handle multiple SkinnedMeshRenderers

#if UNITY_EDITOR
    [Button]
    public void BakeMesh()
    {
        var mesh = new Mesh();
        _lineRenderer.BakeMesh(mesh, SceneView.GetAllSceneCameras()[0]);
        var path = EditorUtility.SaveFilePanelInProject("New Mesh", "New Mesh", "asset", "Save the baked LineRenderer mesh");
        if (!string.IsNullOrEmpty(path))
            AssetDatabase.CreateAsset(mesh, path);
    }

    [Button]
    public void BakeSkinnedMeshes()
    {
        if (_skinnedMeshRenderers == null || _skinnedMeshRenderers.Count == 0)
        {
            Debug.LogWarning("No SkinnedMeshRenderers assigned to bake.");
            return;
        }

        var combinedMesh = new Mesh();
        var combineInstances = new List<CombineInstance>();

        foreach (var skinnedMeshRenderer in _skinnedMeshRenderers)
        {
            if (skinnedMeshRenderer == null) continue;

            var bakedMesh = new Mesh();
            skinnedMeshRenderer.BakeMesh(bakedMesh);

            // Adjust the vertices of the baked mesh to align with the root object's origin
            Vector3[] vertices = bakedMesh.vertices;
            Vector3 worldPosition = skinnedMeshRenderer.transform.position;
            Quaternion worldRotation = skinnedMeshRenderer.transform.rotation;

            for (int i = 0; i < vertices.Length; i++)
            {
                // Transform vertex to world space, then back to root object's local space
                vertices[i] = transform.InverseTransformPoint(worldPosition + worldRotation * vertices[i]);
            }

            bakedMesh.vertices = vertices;
            bakedMesh.RecalculateBounds();

            var combineInstance = new CombineInstance
            {
                mesh = bakedMesh,
                transform = Matrix4x4.identity // No transformation matrix needed
            };
            combineInstances.Add(combineInstance);
        }

        combinedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

        var path = EditorUtility.SaveFilePanelInProject("Combined Mesh", "CombinedMesh", "asset", "Save the combined SkinnedMeshRenderer mesh");
        if (!string.IsNullOrEmpty(path))
            AssetDatabase.CreateAsset(combinedMesh, path);
    }
#endif
}
