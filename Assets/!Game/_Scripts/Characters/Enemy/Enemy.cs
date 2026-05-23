using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, ICompanionAttackable
{
    [Dependency][SerializeField] GameEvent OnNavMeshRebakeGE;
    private NavMeshAgent nma;

    public Vector3 Position => transform.position;

    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();

    }
    private void OnEnable()
    {
        OnNavMeshRebakeGE.Action += WaitForNavMesh;
    }
    private void OnDisable()
    {
        OnNavMeshRebakeGE.Action -= WaitForNavMesh;
    }
    private void WaitForNavMesh()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForNavMeshCoroutine());
    }

    private IEnumerator WaitForNavMeshCoroutine()
    {
        nma.enabled = false;
        while (!nma.NavMeshExists())
        {
            yield return null;
        }
        nma.enabled = true;
    }
    private void Update()
    {
        if (nma.enabled && nma.NavMeshExists()) nma.SetDestination(PlayerData.Position);
    }

    public void Damage()
    {
        gameObject.SetActive(false);
    }
}
