using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour, ICompanionAttackable
{
    [SerializeField] private GameObject _knockedChaserPrefab;
    [SerializeField] private Transform _knockedChaserSpawnPosition;
    [SerializeField] private GameObject _rootObject;
    [SerializeField] private NavMeshAgent _agent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            PlayerManager.Die(null);
        }
    }
    [ContextMenu("Chase()")]
    public void Chase()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Chase());
        IEnumerator Co_Chase()
        {
            while (true)
            {
                _agent.SetDestination(PlayerData.Position);
                yield return null;
            }
        }
    }
    public void Damage()
    {
        var knocked = Instantiate(_knockedChaserPrefab);
        knocked.transform.position = _knockedChaserSpawnPosition.position;
        knocked.transform.rotation = _knockedChaserSpawnPosition.rotation;
        knocked.SetActive(true);
        _rootObject.SetActive(false);

    }
    public Vector3 Position => transform.position;
}
public interface ICompanionAttackable
{
    void Damage();
    Vector3 Position { get; }
}
