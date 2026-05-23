using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshRebake : MonoBehaviour
{
    private static NavMeshRebake instance;
    private AsyncOperation _operation;
    private NavMeshSurface _nms;
    private List<NavMeshBuildSource> _sources = new();
    public static bool BuiltNavMesh { get; private set; }
    private void Awake()
    {
        BuiltNavMesh = true;
        instance = this;
    }
    //private IEnumerator Start()
    //{
    //    _nms = GetComponent<NavMeshSurface>();
    //    yield return null;
    //    Rebake();
    //}
    public static void Rebake()
    {
        return;
        BuiltNavMesh = false;
        if (instance == null) return;
        NavMesh.RemoveAllNavMeshData();
        Bounds bounds = new();
        NavMeshData data = NavMeshBuilder.BuildNavMeshData(instance._nms.GetBuildSettings(), instance._sources, bounds, instance._nms.transform.position, Quaternion.Euler(Vector3.up));
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.RebakeCoroutine(data));
    }
    private IEnumerator RebakeCoroutine(NavMeshData data)
    {
        _operation = _nms.UpdateNavMesh(data);
        while (!_operation.isDone)
        {
            yield return null;
        }
        NavMesh.AddNavMeshData(data);
        BuiltNavMesh = true;
    }
}
