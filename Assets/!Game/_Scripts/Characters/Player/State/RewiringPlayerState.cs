using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Rewiring", fileName = "Rewiring Player State")]
public class RewiringPlayerState : PlayerState, ILookSelectorListener
{
    public static InputActionMap[] Actions;
    public static Rewire Rewire { get; set; }

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private SFX _startSFX;
    [SerializeField] private SFX _endSFX;
    [SerializeField] private SFX _failSFX;
    public SFX EndSFX => _endSFX;
    public SFX FailSFX => _failSFX;

    private Camera _camera;

    private static IScannerSelectable _scanObject;
    public static IScannerSelectable ScanObject => _scanObject;

    private readonly string[] disabledControls = { "Move", "Jump", "Command", "Follow", "Interact", "Sprint" };

    protected override void OnStateEnable()
    {
        PlayerLookSelector.SecondaryInputListenerList.AddListener(this);
        _context.PlayerInput.actions.SetMaps(InputMaps.Default);
        _context.PlayerInput.actions.Link("Glasses", OnDefaultMode);

        if (PlayerStateManager.PreviousState != PlayerStates.Menu)
            _startSFX?.PlayAtPoint(Vector3.zero);


        _camera = PlayerLook.Camera;
        _context.PlayerInput.actions.SetEnabled(disabledControls, false);


    }

    public bool ValidateInputAction() => _scanObject != null;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (_scanObject == null) return;
        if (ctx.performed) { _scanObject.Click(); }
        if (ctx.action.WasReleasedThisFrame()) { _scanObject.UnClick(); }
    }

    protected override void OnStateDisable(PlayerState destinationState)
    {
        if (_scanObject != null)
        {
            Select(null);
            OnStopHover?.Invoke();
        }
        _context.PlayerInput.actions.UnLink("Glasses", OnDefaultMode);
        _context.PlayerInput.actions.SetEnabled(disabledControls, true);
        PlayerLookSelector.SecondaryInputListenerList.RemoveListener(this);
    }

    private void OnDefaultMode(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasReleasedThisFrame()) return;
        PlayerStateManager.SwitchState(PlayerStates.Default);
        PlayerStates.Scanner.EndSFX.PlayAtPoint(Vector3.zero);
    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    public override void OnUpdate()
    {
        if (GameObjectUtility.TryRaycastGetComponent(_camera, _layerMask, out IScannerSelectable scanObject))
        {
            if (_scanObject != scanObject)
            {
                Select(scanObject);
            }
        }
        else if (_scanObject != null)
        {
            Select(null);
        }

    }

    private void Select(IScannerSelectable scanObject)
    {
        if (_scanObject != null)
        {
            _scanObject.UnClick();
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

}

