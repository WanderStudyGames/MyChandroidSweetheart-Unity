using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Tablet : PlayerComponent
{
    private PlayerInput _playerInput;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        _playerInput.actions.Link("Tablet", OnTabletEnable);
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Tablet", OnTabletEnable);
    }
    private void OnTabletEnable(InputAction.CallbackContext ctx)
    {
        if (ctx.started && PlayerStateManager.State == PlayerStates.Default)
        {
            TabletPlayerState.CompanionHeld = false;
            PlayerStateManager.SwitchState(PlayerStates.Tablet);
            Debug.Log("tablet");
        }
    }

    public override void SetComponentProfile(ComponentProfile profile)
    {
    }
}
