using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Menu", fileName = "Menu Player State")]
public class MenuPlayerState : PlayerState
{
    public static InputActionMap[] Actions;

    protected override void OnStateEnable()
    {
        _context.PlayerInput.actions.SetMaps(InputMaps.Menu);
        Cursor.lockState = CursorLockMode.None;

    }
    protected override void OnStateDisable(PlayerState destinationState)
    {
        if (destinationState == null) return;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

