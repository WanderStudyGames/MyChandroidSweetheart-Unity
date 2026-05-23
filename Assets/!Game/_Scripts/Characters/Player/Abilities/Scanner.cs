using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStateManager))]
public class Scanner : PlayerComponent
{

    [SerializeField] private ScannerProfile scannerProfile;

    [SerializeField] private InputActionReference _switchMode;
    private PlayerInput _playerInput;
    private IScannerSelectable _scanObject;
    private Camera _camera;
    public override void SetComponentProfile(ComponentProfile profile)
    {
        scannerProfile = (ScannerProfile)profile;
    }

    private static Scanner instance;

    private AudioSource audioSource;

    private void OnEnable()
    {
        _playerInput.actions.Link("Glasses", OnOverlayToggleInput);

        instance = this;

        Dialogue.OnCharacterSpeak += SwitchToDefault;
        SwitchToDefault();
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Glasses", OnOverlayToggleInput);

        instance = null;

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
        _camera = GetComponent<PlayerLook>().GetCamera();
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
}
