using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Dialogue", fileName = "Dialogue Player State")]
public class DialoguePlayerState : PlayerState
{
    public static InputActionMap[] Actions;

    protected override void OnStateEnable()
    {
        _context.PlayerInput.actions.SetMaps(InputMaps.Dialogue);
        _context.PlayerInput.actions.Link("PageSkip", OnPageSkipInput);

    }
    protected override void OnStateDisable(PlayerState destinationState)
    {
        _context.PlayerInput.actions.UnLink("PageSkip", OnPageSkipInput);
    }
    public event Action OnPageSkip;
    private void OnPageSkipInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame()) { OnPageSkip?.Invoke(); }
    }
}

