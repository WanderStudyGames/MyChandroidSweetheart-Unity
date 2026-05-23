using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    [SerializeField] private LayerMask layerMask = 2147483647;
    [Tooltip("Makes sure all colliders leave the trigger area before the enter or exit events are invoked. \n(May cause issues if game objects are disabled manually.)")]
    [SerializeField] private bool trackColliders;
    [Tooltip("If set, only collisions with this target collider will invoke the events.")]
    [SerializeField] private Collider _target;
    private List<Collider> _colliders = new();
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if ((_target != null && _target != other) || (_target == null && !layerMask.Contains(other.gameObject.layer))) return;


        if (_colliders.Count == 0 || !trackColliders)
        {
            if (isActiveAndEnabled)
                onEnter.Invoke();
        }
        _colliders.Add(other);
        if (other.TryGetComponent(out PhysicsFriendlyDisabler pfd))
        {
            pfd.BeforeDisable += DisableObject;
            _physicsFriendlyDisablers.Add(pfd);
        }
    }
    private void DisableObject(PhysicsFriendlyDisabler disabler)
    {
        for (var i = _colliders.Count - 1; i >= 0; i--)
        {
            var col = _colliders[i];
            if (col.gameObject == disabler.gameObject)
            {
                OnTriggerExit(col);
                disabler.BeforeDisable -= DisableObject;
            }
        }

    }
    private List<PhysicsFriendlyDisabler> _physicsFriendlyDisablers = new();
    void OnDisable()
    {
        foreach (var pfd in _physicsFriendlyDisablers)
        {
            pfd.BeforeDisable -= DisableObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((_target != null && _target != other) || (_target == null && !layerMask.Contains(other.gameObject.layer))) return;

        _colliders.Remove(other);
        if (_colliders.Count == 0 || !trackColliders)
        {
            if (isActiveAndEnabled)
                onExit.Invoke();
        }
    }
}
