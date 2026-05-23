using UnityEngine;
using UnityEngine.Events;

public class CEnemyDetection : MonoBehaviour
{
    [SerializeField] private Companion cmp;
    [SerializeField] private UnityEvent<CompanionReactible> _onReactibleEnter;
    [SerializeField] private UnityEvent<CompanionReactible> _onReactibleExit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICompanionAttackable companionAttackable))
        {
            cmp.Attack(companionAttackable);
        }
        else if (other.TryGetComponent(out CompanionReactible reactible) && reactible.enabled)
        {
            _onReactibleEnter?.Invoke(reactible);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CompanionReactible reactible) && reactible.enabled)
        {
            _onReactibleExit?.Invoke(reactible);
        }
    }
}
