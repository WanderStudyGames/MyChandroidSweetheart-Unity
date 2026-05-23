using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Frozen Dialogue", fileName = "Frozen Dialogue Player State")]
public class DialogueFrozenPlayerState : PlayerState
{
    public static InputActionMap[] Actions;

    protected override void OnStateEnable()
    {
        _context.PlayerInput.actions.SetMaps(InputMaps.Menu);
        _context.PlayerInput.actions.Link("Confirm", OnPageSkipInput);
        _context.PlayerInput.actions["Pause"].Enable();
    }

    protected override void OnStateDisable(PlayerState destinationState)
    {
        Debug.Log("disabling Frozen");
        _context.PlayerInput.actions["Pause"].Disable();
        _context.PlayerInput.actions.UnLink("Confirm", OnPageSkipInput);//
    }
    public event Action OnPageSkip;
    private void OnPageSkipInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            OnPageSkip?.Invoke();
        }
    }
}

