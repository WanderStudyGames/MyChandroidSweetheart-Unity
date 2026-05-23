using System;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
public class Interact : PlayerComponent, ILookSelectorListener
{
    [SerializeField] private InteractProfile interactProfile;
    [SerializeField] private PlayerInput _playerInput;
    private AudioSource audioSource;
    private Camera _camera;
    private InteractibleObject _interactObject;
    private ExecuteOnLookedAt _lookAtTrigger;
    public bool ValidateInputAction() => _interactObject != null;
    private PlayerLookSelector _playerLookSelector;
    public override void SetComponentProfile(ComponentProfile profile)
    {
        interactProfile = (InteractProfile)profile;
    }
    private void OnEnable()
    {
        // _playerInput.actions.Link("Primary", OnInteract);
        _playerLookSelector.OnTriggerHit += OnRaycastTrigger;
        PlayerLookSelector.PrimaryInputListenerList.AddListener(this);

        PlayerStateManager.OnStateChanged += OnPlayerStateChanged;
    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void OnDisable()
    {
        if (_interactObject != null)
        {
            OnStopHover?.Invoke();
            _interactObject = null;
        }
        //_playerInput.actions.UnLink("Primary", OnInteract);
        _playerLookSelector.OnTriggerHit -= OnRaycastTrigger;
        PlayerLookSelector.PrimaryInputListenerList.RemoveListener(this);

        PlayerStateManager.OnStateChanged -= OnPlayerStateChanged;
    }
    private void Awake()
    {
        _camera = GetComponent<PlayerLook>().GetCamera();
        _playerLookSelector = GetComponent<PlayerLookSelector>();
    }

    public void InteractAction()
    {

        if (_interactObject != null)
        {
            _interactObject.Interact();
        }

    }
    void OnPlayerStateChanged(PlayerState state)
    {
        if (state != PlayerStates.Default && state != PlayerStates.Scanner)
            OnStopHover?.Invoke();
    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    private void OnRaycastTrigger(RaycastHit hit)
    {
        if (hit.collider != null && hit.collider.TryGetComponent(out ExecuteOnLookedAt lookAt))
        {
            if (_lookAtTrigger != lookAt)
            {
                lookAt.LookAt();
                _lookAtTrigger = lookAt;
            }
        }
        else if (_lookAtTrigger != null)
        {
            _lookAtTrigger = null;
        }
        bool inState = PlayerStateManager.State == PlayerStates.Default || PlayerStateManager.State == PlayerStates.Scanner;
        if (inState && hit.TryGetComponent(out InteractibleObject interactibleObject, interactProfile.MaxDistance, interactProfile.InteractLayerMask) && interactibleObject.CanInteract())
        {
            if (_interactObject != interactibleObject)
            {
                _interactObject = interactibleObject;
            }
            OnHover?.Invoke(interactibleObject.Icon);
        }
        else if (_interactObject != null)
        {
            OnStopHover?.Invoke();
            _interactObject = null;
        }
    }
    private bool pressed;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            InteractAction();
        }
        return;
        if (ctx.action.WasPressedThisFrame() && !pressed)
        {
            InteractAction();
            pressed = true;
        }
        if (ctx.action.WasReleasedThisFrame() || ctx.canceled)
        {
            pressed = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (_camera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_camera.transform.position, _camera.transform.position + (_camera.transform.forward * interactProfile.MaxDistance));
        }
    }

}
