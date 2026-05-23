#define QC_DISABLE_BUILTIN_ALL
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private QuantumConsole _quantumConsole;
    bool _isPaused = false;
    private void OnEnable()
    {
        _actionReference.action.Link(OnPause);
        _actionReference.action.Enable();
    }
    private void OnDisable()
    {
        _actionReference.action.UnLink(OnPause);
    }
    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (!(ctx.started && ctx.action.WasPressedThisFrame())) return;
        if (!_pauseMenu.IsPaused) { _pauseMenu.Pause(); }
    }



}
