using System.Collections;
using UnityEngine;

public class CompanionSpawnTrigger : MonoBehaviour
{
    [SerializeField] private ExecuteUnityEvent interactObject;
    private enum StartingBehaviors
    {
        Follow,
        Idle
    }
    [SerializeField] private StartingBehaviors _behavior = StartingBehaviors.Follow;
    public static void ResetSpawnFlag() { _spawned = false; }
    private static bool _spawned;
    private void Awake()
    {
        _spawned = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            StopAllCoroutines();
            StartCoroutine(Co_Spawn());
        }
    }

    private IEnumerator Co_Spawn()
    {
        yield return new WaitUntil(() => NavMeshRebake.BuiltNavMesh);
        if (CompanionManager.CompanionGOExists || _spawned || !CompanionManager.CompanionWithPlayer) yield break;
        if (interactObject == null) interactObject = CompanionManager.DefaultInteractObject;
        CompanionManager.Spawn(transform.position, transform.rotation);
        CompanionManager.SetInteractObject(interactObject);
        yield return new WaitUntil(() => CompanionManager.CompanionGOExists);
        yield return null;
        _spawned = true;
        if (_behavior == StartingBehaviors.Idle)
        {
            CompanionManager.StandInPlace();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawRay(Vector3.zero, Vector3.forward);
        Gizmos.DrawWireSphere(Vector3.zero, 0.3f);
        Gizmos.DrawSphere(Vector3.zero, 0.2f);
        Gizmos.DrawSphere(Vector3.forward, 0.05f);
        var color = Gizmos.color;
        color.a = 0.1f;
        Gizmos.color = color;
        Gizmos.DrawCube(Vector3.up, new(1, 2, 1));
    }
}
