using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ChainsawHead : MonoBehaviour
{
    [SerializeField] private RotateOverTime _rotator;
    [SerializeField] private RotateRandom _rotateRandom;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private int _health = 3;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onDie;
    private void Start()
    {
        Setup();
    }
    private void Setup()
    {
        StopAllCoroutines();
        switch (_health)
        {
            case 3:
                StartCoroutine(Co_Three());
                break;
            case 2:

                StartCoroutine(Co_Two());
                break;
            case 1:
                StartCoroutine(Co_One());
                break;
            default:
                break;
        }
        IEnumerator Co_Three()
        {
            while (true)
            {
                _rotateRandom.RotateRandomY();
                yield return new WaitForSeconds(2f);
            }
        }
        IEnumerator Co_Two()
        {
            while (true)
            {
                _rotateRandom.RotateRangedYTowardsCompanion();
                yield return new WaitForSeconds(2f);
            }
        }
        IEnumerator Co_One()
        {
            while (true)
            {
                _rotateRandom.RotateRangedYTowardsCompanion();
                var awayFromCompanion = transform.position - CompanionData.Position;
                _navMeshAgent.SetDestination(transform.position + awayFromCompanion);
                yield return new WaitForSeconds(2.3f);
            }
        }
    }
    public void TakeDamage()
    {
        StopAllCoroutines();
        _health--;
        if (_health <= 0)
        {
            _onTakeDamage.Invoke();
            _onDie.Invoke();
            return;
        }
        _onTakeDamage.Invoke();
        Setup();
    }
}
