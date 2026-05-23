using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
public class PlayerLookSelector : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SFX _primaryActionFailSFX;
    [SerializeField] private SFX _secondaryActionFailSFX;

    [SerializeField] private InputActionReference _primaryAction;
    [SerializeField] private InputActionReference _secondaryAction;

    private AudioSource _audioSource;

    public static InputListenerList PrimaryInputListenerList = new();
    public static InputListenerList SecondaryInputListenerList = new();

    public event Action<RaycastHit> OnTriggerHit;
    public event Action<RaycastHit> OnColliderHit;

    public static event Action<Sprite> OnPrimaryHover { add { PrimaryInputListenerList.OnHover += value; } remove { PrimaryInputListenerList.OnHover -= value; } }
    public static event Action OnPrimaryStopHover { add { PrimaryInputListenerList.OnStopHover += value; } remove { PrimaryInputListenerList.OnStopHover -= value; } }
    public static event Action<Sprite> OnSecondaryHover { add { SecondaryInputListenerList.OnHover += value; } remove { SecondaryInputListenerList.OnHover -= value; } }
    public static event Action OnSecondaryStopHover { add { SecondaryInputListenerList.OnStopHover += value; } remove { SecondaryInputListenerList.OnStopHover -= value; } }

    private static readonly LayerMask _layerMask = new LayerMask() { value = Physics.AllLayers }.Exclude(2).Exclude(5).Exclude(14).Exclude(8).Exclude(4);
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<PlayerLook>().GetCamera();
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        _primaryAction.action.Link(OnPrimary);
        _secondaryAction.action.Link(OnSecondary);
    }

    private void OnDisable()
    {

        _primaryAction.action.UnLink(OnPrimary);
        _secondaryAction.action.UnLink(OnSecondary);

    }
    private void OnPrimary(InputAction.CallbackContext ctx)
    {
        if (!PrimaryInputListenerList.TryDoAction(ctx))
        {
            _audioSource.PlaySFX(_primaryActionFailSFX);
        }
    }
    private void OnSecondary(InputAction.CallbackContext ctx)
    {

        if (!SecondaryInputListenerList.TryDoAction(ctx))
        {
            if (PlayerStateManager.State == PlayerStates.Default && Inventories.Instance.PlayerInventory.Has("Sprocket Gun")) return;
            _audioSource.PlaySFX(_secondaryActionFailSFX);
        }
    }


    private void Update()
    {
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 1000f, _layerMask, QueryTriggerInteraction.Collide);
        OnTriggerHit?.Invoke(hit);
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 1000f, _layerMask, QueryTriggerInteraction.Ignore);
        OnColliderHit?.Invoke(hit);
    }
}

public interface ILookSelectorListener
{
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    public void OnInputAction(InputAction.CallbackContext ctx);
    public bool ValidateInputAction();
}

public class InputListenerList
{
    private List<ILookSelectorListener> _inputListeners = new();
    public event Action<Sprite> OnHover;
    private void OnHoverMethod(Sprite s) { OnHover?.Invoke(s); }
    public event Action OnStopHover;
    private void OnStopHoverMethod() { OnStopHover?.Invoke(); }
    public void AddListener(ILookSelectorListener listener)
    {
        _inputListeners.AddUnique(listener);
        listener.OnHover += OnHoverMethod;
        listener.OnStopHover += OnStopHoverMethod;
    }
    public void RemoveListener(ILookSelectorListener listener)
    {
        _inputListeners.RemoveAll(listener);
        listener.OnHover -= OnHoverMethod;
        listener.OnStopHover -= OnStopHoverMethod;
    }
    public bool TryDoAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            for (int i = _inputListeners.Count - 1; i >= 0; i--)
            {
                var listener = _inputListeners[i];
                if (listener.ValidateInputAction())
                {
                    listener.OnInputAction(ctx);
                    return true;
                }
            }
        }
        else
        {
            for (int i = _inputListeners.Count - 1; i >= 0; i--)
            {
                _inputListeners[i].OnInputAction(ctx);
            }
            return true;
        }
        return false;
    }
}