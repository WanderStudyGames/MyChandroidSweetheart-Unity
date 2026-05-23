using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Default", fileName = "Default Player State")]
public class DefaultPlayerState : PlayerState
{
    public static InputActionMap[] Actions;

    protected override void OnStateEnable()
    {
        if (PlayerStateManager.PreviousState != PlayerStates.Carrying)
            if (_context.PlayerInput != null)
                _context.PlayerInput.actions.SetMaps(InputMaps.Default);
    }

}

