using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Scanner", fileName = "Scanner Player State")]
public class ScannerPlayerState : PlayerState, ILookSelectorListener
{
    public static InputActionMap[] Actions;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private SFX _startSFX;
    [SerializeField] private SFX _endSFX;
    public SFX EndSFX => _endSFX;
    [SerializeField] private SFX _ambienceSFX;
    private AudioSource _ambienceAudioSource;

    private Camera _camera;
    private IScannerSelectable _scanObject;


    protected override void OnStateEnable()
    {
        if (PlayerStateManager.PreviousState != PlayerStates.Default) _context.PlayerInput.actions.SetMaps(InputMaps.Default);

        _context.PlayerLookSelector.OnTriggerHit += OnRaycastTrigger;
        PlayerLookSelector.SecondaryInputListenerList.AddListener(this);

        if (PlayerStateManager.PreviousState == PlayerStates.Default)
        {
            _startSFX.PlayAtPoint(Vector3.zero);
        }

        Debug.Assert(_context != null);
        Debug.Log(_context);


        _camera = PlayerLook.Camera;
        _scanObject = null;
    }
    protected override void OnStateDisable(PlayerState destinationState)
    {

        if (destinationState != PlayerStates.Scanning && destinationState != PlayerStates.Menu && destinationState != null)
        {
            _endSFX.PlayAtPoint(Vector3.zero);
        }
        if (_scanObject != null)
        {
            Select(null);
            OnStopHover?.Invoke();
        }

        PlayerLookSelector.SecondaryInputListenerList.RemoveListener(this);
        Debug.Assert(_context != null);
        _context.PlayerLookSelector.OnTriggerHit -= OnRaycastTrigger;

    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    public void OnRaycastTrigger(RaycastHit hit)
    {
        if (hit.TryGetComponent(out IScannerSelectable iss, layerMask: _layerMask) && iss.Enabled)
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
    public bool ValidateInputAction() => _scanObject != null;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (_scanObject == null) return;
        if (ctx.performed) { _scanObject.Click(); }
        if (ctx.action.WasReleasedThisFrame()) { _scanObject.UnClick(); }
    }



}

