using System;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStateManager))]
public class Scanner : PlayerComponent, ILookSelectorListener
{

    [SerializeField] private ScannerProfile scannerProfile;

    [SerializeField] private InputActionReference _switchMode;
    private PlayerInput _playerInput;
    private IScannerSelectable _scanObject;
    private PlayerLookSelector _playerLookSelector;
    public override void SetComponentProfile(ComponentProfile profile)
    {
        scannerProfile = (ScannerProfile)profile;
    }

    private static Scanner instance;

    private AudioSource audioSource;

    private void OnEnable()
    {
        //_playerInput.actions.Link("Glasses", OnOverlayToggleInput);

        instance = this;
        PlayerLookSelector.SecondaryInputListenerList.AddListener(this);
        _playerLookSelector.OnTriggerHit += OnRaycastTrigger;
        Debug.Assert(_playerLookSelector != null);
        Dialogue.OnCharacterSpeak += SwitchToDefault;
        SwitchToDefault();
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Glasses", OnOverlayToggleInput);

        instance = null;
        PlayerLookSelector.SecondaryInputListenerList.RemoveListener(this);
        _playerLookSelector.OnTriggerHit -= OnRaycastTrigger;
        Dialogue.OnCharacterSpeak -= SwitchToDefault;
    }
    private void OnDestroy()
    {
        Destroy(audioSource);
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        _playerInput = GetComponent<PlayerInput>();
        _playerLookSelector = GetComponent<PlayerLookSelector>();
        // _camera = GetComponent<PlayerLook>().GetCamera();
    }
    public void OnOverlayToggleInput(InputAction.CallbackContext ctx)
    {
        if (!(ctx.action.WasReleasedThisFrame() && ctx.action.ReadValue<float>() < 0.1f)) return;
        if (PlayerStateManager.State == PlayerStates.Default)
        {
            PlayerStateManager.SwitchState(PlayerStates.Scanner);
        }
        else if (PlayerStateManager.State == PlayerStates.Scanner)
        {
            PlayerStateManager.SwitchState(PlayerStates.Default);
        }
    }

    public void OnEnableDefaultMode(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasPressedThisFrame()) return;

        PlayerStateManager.SwitchState(PlayerStates.Default);

        SwitchToDefault();
    }
    void SwitchToDefault(Dialogue dialogue) { SwitchToDefault(); }
    public void SwitchToDefault()
    {
        if (PlayerStateManager.State == PlayerStates.Scanner)
            PlayerStateManager.SwitchState(PlayerStates.Default);
        PostProcProfileController.ResetProfile();
    }
    public void OnRaycastTrigger(RaycastHit hit)
    {
        if (hit.TryGetComponent(out IScannerSelectable iss, layerMask: scannerProfile.LayerMask) && iss.Enabled)
        {
            if (_scanObject != iss)
            {
                Select(iss);
            }
        }
        else if (_scanObject != null)
        {
            _scanObject.Deselect();
            Select(null);
        }
    }
    private void Select(IScannerSelectable scanObject)
    {
        if (_scanObject != null)
        {
            _scanObject.UnClick();
            if (scanObject != null)
                _scanObject.Deselect();
        }
        _scanObject = scanObject;
        if (scanObject == null)
        {
            OnStopHover?.Invoke();
            _scanObject = null;
        }
        else if (scanObject.Select())
        {
            OnHover?.Invoke(scanObject.Icon);
        }
    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    public bool ValidateInputAction() => _scanObject != null;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (_scanObject == null) return;
        if (ctx.performed) { _scanObject.Click(); }
        if (ctx.action.WasReleasedThisFrame()) { _scanObject.UnClick(); }
    }
}
