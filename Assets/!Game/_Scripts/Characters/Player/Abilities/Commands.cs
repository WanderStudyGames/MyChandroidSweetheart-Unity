using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
public class Commands : PlayerComponent
{
    //[Dependency]
    //[SerializeField] private GameEvent OnCommandGE;

    //[Space(30)]
    //[SerializeField] private LayerMask LayerMask;
    private static GameObject locIn;
    private static GameObject folIn;

    private CompanionInteractible _companionInteractible;

    private static RaycastHit _colliderHit;

    private AudioSource audioSource;

    private PlayerLookSelector _playerLookSelector;

    [SerializeField] private CommandsProfile commandsProfile;
    private PlayerInput _playerInput;

    public override void SetComponentProfile(ComponentProfile profile)
    {
        commandsProfile = (CommandsProfile)profile;
    }


    private Camera _camera;

    private void OnEnable()
    {
        _playerInput.actions.Link("Command", OnCommandLocation);
        _playerInput.actions.Link("Follow", OnCommandFollow);

        _playerLookSelector.OnTriggerHit += OnRaycastTrigger;
        _playerLookSelector.OnColliderHit += OnRaycastCollider;

        PlayerStateManager.OnStateChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Command", OnCommandLocation);
        _playerInput.actions.UnLink("Follow", OnCommandFollow);

        _playerLookSelector.OnTriggerHit -= OnRaycastTrigger;
        _playerLookSelector.OnColliderHit -= OnRaycastCollider;

        PlayerStateManager.OnStateChanged -= OnStateChanged;

        if (_companionInteractible != null)
        {
            OnStopHoverCompanionInteractible?.Invoke();
            _companionInteractible = null;
        }
    }
    private void Awake()
    {
        _camera = GetComponent<PlayerLook>().GetCamera();
        _playerInput = GetComponent<PlayerInput>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        _playerLookSelector = GetComponent<PlayerLookSelector>();
    }

    public void OnCommandLocation(InputAction.CallbackContext ctx)
    {
        if (enabled)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                CommandLocation();
            }
        }
    }
    public void OnCommandFollow(InputAction.CallbackContext ctx)
    {
        if (enabled)
        {
            if (ctx.action.WasPressedThisFrame())
            {
                CommandFollow();
            }
        }
    }

    void KillGOSelect()
    {
        if (CompanionData.CurrentCommand != null && CompanionData.CurrentCommand.Transform != null)
        {
            var go = CompanionData.CurrentCommand.Transform;
            if (go.TryGetComponent(out CGameObjectSelect cgos))
            {
                cgos.Off();
            }
        }
    }
    public static event Action OnCommand;
    private void CommandLocation()
    {
        KillGOSelect();
        //Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 100f, commandsProfile.LayerMask, QueryTriggerInteraction.Ignore);

        if (_companionInteractible != null)
        {
            CompanionData.Command(_companionInteractible.gameObject.transform);

            commandsProfile.OnCommandGE.Raise();
            OnCommand?.Invoke();
            audioSource.PlaySFX(commandsProfile.locationSFX);
            KillIndicator();
            if (_companionInteractible.gameObject.TryGetComponent(out CGameObjectSelect cgos))
            {
                cgos.On();
            }
        }
        else
        {
            if (_colliderHit.distance > 0 && commandsProfile.LayerMask.Contains(_colliderHit.collider.gameObject.layer))
            {
                CompanionData.Command(_colliderHit.point);
                KillIndicator();
                SummonIndicator(_colliderHit);
                commandsProfile.OnCommandGE.Raise();
                OnCommand?.Invoke();
                audioSource.PlaySFX(commandsProfile.locationSFX);
            }
        }
    }
    private void SummonIndicator(RaycastHit hit)
    {
        locIn = Instantiate(commandsProfile.LocationIndicatorPF);
        locIn.transform.position = hit.point;
        locIn.transform.rotation = Quaternion.LookRotation(hit.normal);
    }
    private void KillIndicator()
    {
        if (locIn != null) { Destroy(locIn); }
        locIn = null;
    }
    private void CommandFollow()
    {
        KillGOSelect();
        KillIndicator();
        if (folIn == null)
        {
            folIn = Instantiate(commandsProfile.FollowIndicatorPF);
            folIn.transform.SetParent(transform, false);
            folIn.transform.localPosition = Vector3.zero;
            folIn.transform.rotation = Quaternion.identity;
            audioSource.PlaySFX(commandsProfile.followSFX);
        }
        CompanionData.Command(this.transform);
        //commandsProfile.OnCommandGE.Raise();
        OnCommand?.Invoke();
    }
    private void OnStateChanged(PlayerState state)
    {
        if (state != PlayerStates.Default && state != PlayerStates.Scanner)
            OnStopHoverCompanionInteractible?.Invoke();
        if (state == PlayerStates.Tablet)
        {
            _playerInput.actions.UnLink("Follow", OnCommandFollow);
        }
        else if (PlayerStateManager.PreviousState == PlayerStates.Tablet)
        {
            _playerInput.actions.Link("Follow", OnCommandFollow);
        }
    }
    public static event Action OnHoverCompanionInteractible;
    public static event Action OnStopHoverCompanionInteractible;
    private void OnRaycastTrigger(RaycastHit hit)
    {
        if (PlayerStateManager.State == PlayerStates.Scanning) return;
        if (PlayerStateManager.State == PlayerStates.DialogueFrozen) return;
        if (hit.TryGetComponent(out CompanionInteractible coInt, layerMask: commandsProfile.LayerMask))
        {
            if (_companionInteractible != coInt)
            {
                if (coInt.IsInteractable())
                {
                    _companionInteractible = coInt;
                    OnHoverCompanionInteractible?.Invoke();
                }
                else
                {
                    OnStopHoverCompanionInteractible?.Invoke();
                    _companionInteractible = null;
                }
            }
        }
        else if (_companionInteractible != null)
        {
            OnStopHoverCompanionInteractible?.Invoke();
            _companionInteractible = null;
        }//
    }
    private void OnRaycastCollider(RaycastHit hit)
    {
        _colliderHit = hit;
    }
}
