using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerState : ScriptableObject
{
    public event Action OnStateEnableEvent;
    public event Action OnStateDisableEvent;
    protected PlayerStateManager _context;
    public void Transition(PlayerState state) { PlayerStateManager.SwitchState(state); }
    public void Enable(PlayerStateManager context)
    {
        if (_context != null) return;
        _context = context;
        OnStateEnableEvent?.Invoke();
        OnStateEnable();
    }
    protected virtual void OnStateEnable() { }
    public void Disable(PlayerState destinationState)
    {
        if (_context == null) return;
        OnStateDisable(destinationState);
        OnStateDisableEvent?.Invoke();
        _context = null;
    }
    public virtual void OnUpdate() { }
    protected virtual void OnStateDisable(PlayerState destinationState) { }
    protected virtual void OnMove(InputAction.CallbackContext ctx) { }
}