using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Scanning", fileName = "Scanning Player State")]
public class ScanningPlayerState : PlayerState, ILookSelectorListener
{
    public static InputActionMap[] Actions;
    private bool ExitOnRelease;
    public event Action OnPageSkip;
    public event Action OnLeaveScan;
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;

    protected override void OnStateEnable()
    {

        _context.PlayerInput.actions.SetMaps(InputMaps.Menu);
        _context.PlayerInput.actions["Tool"].Enable();
        _context.PlayerInput.actions["Pause"].Enable();
        PlayerLookSelector.SecondaryInputListenerList.AddListener(this);

        _context.PlayerInput.actions.Link("Confirm", OnScanPageSkip);
        ExitOnRelease = false;
        Debug.LogWarning("SCANNING");
        Time.timeScale = 0;

    }
    protected override void OnStateDisable(PlayerState destinationState)
    {
        Time.timeScale = 1;
        PlayerLookSelector.SecondaryInputListenerList.RemoveListener(this);

        _context.PlayerInput.actions.UnLink("Confirm", OnScanPageSkip);
        _context.PlayerInput.actions["Tool"].Disable();
        _context.PlayerInput.actions["Pause"].Disable();
    }
    public void OnScanPageSkip(InputAction.CallbackContext ctx)
    {
        //if (ctx.action.WasPressedThisFrame()) { OnPageSkip?.Invoke(); }
    }
    public void OnLeaveScanInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasReleasedThisFrame()) return;
        ExitOnRelease = true;
        if (ExitOnRelease)
        {
            OnLeaveScan?.Invoke();
            return;
        }
        else { ExitOnRelease = true; }
    }

    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        OnLeaveScanInput(ctx);
    }

    public bool ValidateInputAction()
    {
        return true;
    }
}

