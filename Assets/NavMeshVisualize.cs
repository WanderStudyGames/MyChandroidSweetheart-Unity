using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VInspector;

public class NavMeshVisualize : MonoBehaviour
{
    private List<Vector3> vertices;
    private List<int> triangles;
    [SerializeField] int areaIndex;
    [SerializeField] Material material;
    private static MeshRenderer _meshRenderer;

    [Button("Get Vertices")]
    public void GetVertices()
    {
        var navMeshData = NavMesh.CalculateTriangulation();
        vertices = new List<Vector3>();
        triangles = new List<int>();

        // Dictionary to track unique vertices and their indices
        Dictionary<Vector3, int> vertexIndexMap = new();

        // Iterate through all triangles
        for (int i = 0; i < navMeshData.areas.Length; i++)
        {
            if (navMeshData.areas[i] == areaIndex) // Only process triangles in the specified area
            {
                for (int j = 0; j < 3; j++) // Process each vertex of the triangle
                {
                    Vector3 worldVertex = navMeshData.vertices[navMeshData.indices[i * 3 + j]];
                    Vector3 localVertex = transform.InverseTransformPoint(worldVertex); // Convert to local space

                    // Check if the vertex is already in the list
                    if (!vertexIndexMap.TryGetValue(localVertex, out int vertexIndex))
                    {
                        vertexIndex = vertices.Count;
                        vertices.Add(localVertex);
                        vertexIndexMap[localVertex] = vertexIndex;
                    }

                    // Add the vertex index to the triangles list
                    triangles.Add(vertexIndex);
                }
            }
        }
    }

    private void CreateMesh()
    {
        Debug.Log("Creating mesh with " + vertices.Count + " vertices.");
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.name = "NavMeshAreaMesh";

        Debug.Log("Mesh created with " + mesh.vertexCount + " vertices and " + mesh.triangles.Length / 3 + " triangles.");

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        _meshRenderer = meshRenderer;
        _meshRenderer.enabled = false;
        Debug.Log("Mesh assigned to MeshFilter and MeshRenderer added.");
    }

    public static void EnableVisuals(bool b)
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.enabled = b;
        }
    }

    private void Awake()
    {
        GetVertices();
        CreateMesh();
    }


}
