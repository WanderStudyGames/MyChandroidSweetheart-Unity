using System.Linq;
using UnityEditor;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    //[SerializeField] private UnityEvent _onSpawnHere;
    //public void ExecuteSpawnEvent()
    //{
    //    _onSpawnHere.Invoke();
    //}
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
        if (gameObject.name.ToLower().Contains("debug")) return;
        var startup = FindObjectOfType<SceneStartup>();
        if (startup != null)
        {
            if (!startup.Spawnpoints.Contains(transform))
            {
                startup.AddSpawnpoint(transform);
            }
        }
    }
    [ExecuteInEditMode]
    private void OnDestroy()
    {
        if (Application.isPlaying) return;
        if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
        if (gameObject.name.ToLower().Contains("debug")) return;
        var startup = FindObjectOfType<SceneStartup>();
        if (startup != null)
        {
            if (startup.Spawnpoints.Contains(transform))
            {
                startup.RemoveSpawnpoint(transform);
            }
        }
    }
#endif
    public void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
#if UNITY_EDITOR
        if (gameObject.name == "PlayerDebugSpawn") Gizmos.color = new(0, 1, 0);
        else if (gameObject.name == "CompanionDebugSpawn") Gizmos.color = new(0.7f, 0, 0.5f, 1f);
#endif
        Gizmos.DrawRay(Vector3.zero, Vector3.forward);
        Gizmos.DrawWireSphere(Vector3.zero, 0.3f);
        Gizmos.DrawSphere(Vector3.zero, 0.2f);
        Gizmos.DrawSphere(Vector3.forward, 0.05f);
        var color = Gizmos.color;
        color.a = 0.1f;
        Gizmos.color = color;
#if UNITY_EDITOR
        if (gameObject.name == "PlayerDebugSpawn") Gizmos.color = new(0, 1, 0, 0.5f);
        else if (gameObject.name == "CompanionDebugSpawn") Gizmos.color = new(0.7f, 0, 0.5f, 0.5f);


#endif
        Gizmos.DrawCube(Vector3.up, new(1, 2, 1));
    }
}
