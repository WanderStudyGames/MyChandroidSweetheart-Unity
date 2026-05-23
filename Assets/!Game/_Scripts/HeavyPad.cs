using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeavyPad : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    [SerializeField] private UnityEvent onFail;
    private List<CompanionCarryable> _colliders = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Companion companion))
        {
            if (companion.CarriedObject != null)
            {
                if (_colliders.Count == 0)
                {
                    onEnter.Invoke();
                }
                _colliders.AddUnique(companion.CarriedObject);
            }
            else { onFail.Invoke(); }
        }
        else if (other.CompareTag(PlayerManager.Tag)) { onFail.Invoke(); }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Companion companion))
        {
            if (companion.CarriedObject != null)
            {
                _colliders.RemoveAll(companion.CarriedObject);
                if (_colliders.Count == 0)
                {
                    onExit.Invoke();
                }
            }
        }
    }
}
