using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayerInput : MonoBehaviour
{

    private static GlobalPlayerInput instance;
    public static string ControlScheme { get; private set; } = "Keyboard & Mouse";
    public static void SetControlScheme(string scheme) { if (ControlScheme == scheme) return; ControlScheme = scheme; OnControlsChanged?.Invoke(scheme); }
    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        ControlScheme = _playerInput.currentControlScheme;
    }
    public static event Action<string> OnControlsChanged;
    private void ControlsChangedAction(PlayerInput playerInput)
    {
        ControlScheme = playerInput.currentControlScheme;
        OnControlsChanged?.Invoke(playerInput.currentControlScheme);
    }
    private void OnEnable()
    {
        _playerInput.ActivateInput();
        _playerInput.onControlsChanged += ControlsChangedAction;
    }
    public static void DisableComponentInstance()
    {
        if (instance == null) return;
        instance._playerInput.DeactivateInput();
        instance.StartCoroutine(Co_EndOfFrame());
        IEnumerator Co_EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            _lastControlScheme = instance._playerInput.currentControlScheme;
            _lastInputDevices = instance._playerInput.devices.ToArray();
            instance._playerInput.enabled = false;
        }
    }
    private static string _lastControlScheme = string.Empty;
    private static InputDevice[] _lastInputDevices = { };
    public static void EnableComponentInstance()
    {
        if (instance == null) return;
        instance._playerInput.enabled = true;
        instance._playerInput.SwitchCurrentControlScheme(_lastControlScheme, _lastInputDevices);

    }
    private void OnDisable()
    {
        _playerInput.DeactivateInput();
        _playerInput.onControlsChanged -= ControlsChangedAction;
    }
    [SerializeField] private PlayerInput _playerInput;
    public static PlayerInput Instance => instance != null ? instance._playerInput : null;
}

