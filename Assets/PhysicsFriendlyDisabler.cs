using System;

using UnityEngine;
using UnityEngine.Events;

public class PhysicsFriendlyDisabler : MonoBehaviour
{
    [SerializeField] private bool _destroyAfterDisable;
    [SerializeField] private UnityEvent _onDisabled;
    public event Action<PhysicsFriendlyDisabler> BeforeDisable;
    public void Disable()
    {
        this.PhysicsFriendlyDisable(_destroyAfterDisable, () =>
        {
            _onDisabled.Invoke();
        });
    }
    void OnDisable()
    {
        BeforeDisable?.Invoke(this);
    }
}
