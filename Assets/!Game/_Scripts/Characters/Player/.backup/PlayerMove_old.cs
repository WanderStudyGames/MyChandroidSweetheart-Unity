using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterControllerMove))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerSwim))]
public class PlayerMove : PlayerComponent
{
    private static PlayerMove instance;
    PlayerAudio _playerAudio;
    [SerializeField] private PlayerMovementProfile _playerMovementProfile;
    [SerializeField] private PlayerInput _playerInput;
    public PlayerMovementProfile PlayerMovementProfile => _playerMovementProfile;
    public override void SetComponentProfile(ComponentProfile profile) { _playerMovementProfile = (PlayerMovementProfile)profile; }
    private CharacterControllerMove _characterControllerMove;
    public CharacterControllerMove CharacterControllerMove => _characterControllerMove;

    private bool IsGrounded => _characterControllerMove.IsGrounded;
    private PlayerSwim _playerSwim;
    private bool IsSwimming => _playerSwim.IsSwimming;
    public bool ControlsEnabled => !_disableControls;

    private Vector2 _moveInput;
    public void SetMove(Vector2 dimensions)
    {
        if (dimensions.magnitude > 1)
        {
            _moveInput = dimensions.normalized;
        }
        else _moveInput = dimensions;
    }
    public static event Action OnSprint;
    public static event Action OnWalk;
    public void OnSprintInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasPressedThisFrame()) return;
        if (ctx.ReadValue<float>() > 0)
        {
            _isSprinting = true;
            OnSprint?.Invoke();
        }
        else if (ctx.ReadValue<float>() < 0)
        {
            _isSprinting = false;
            OnWalk?.Invoke();
        }
    }
    public void OnWalkInput(InputAction.CallbackContext ctx)
    {
        _isSprinting = false;
        OnWalk?.Invoke();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.canceled) { SetMove((Vector2)context.ReadValueAsObject()); }
        else { SetMove(Vector2.zero); }
    }
    //horizontal movement ---------------------------------------
    private readonly List<TaggedValue<float>> _speedMultipliers = new();
    public List<TaggedValue<float>> SpeedMultipliers => _speedMultipliers;
    public static List<TaggedValue<float>> SpeedMultis => instance._speedMultipliers;
    [SerializeField] private bool _disableControls = false;
    public void SetDisableControls(bool b) { _disableControls = b; }
    private static bool _isSprinting = true;
    private Vector2 _localHVelocity = Vector2.zero;
    private float MoveSpeed => IsGrounded ? _playerMovementProfile.accelerationGround : _playerMovementProfile.accelerationAir;
    private float StepInterval => IsSwimming ? _playerMovementProfile.swimStrokeDistance : _playerMovementProfile.stepDistance;
    public float GetMaxSpeed()
    {
        float speedLimit = _playerMovementProfile.walkSpeed;
        if (_isSprinting)
        {
            speedLimit = _playerMovementProfile.sprintSpeed;
        }
        foreach (TaggedValue<float> speedMultiplier in _speedMultipliers)
        {
            speedLimit *= speedMultiplier.Value;
            if (speedMultiplier.Value <= 0) { Logger.Error($"{typeof(PlayerMove)}: invalid speed limit {speedMultiplier.Value}"); }
        }
        return speedLimit * _moveInput.magnitude;
    }

    public Vector2 UpdateLocalHVelocity(Vector2 localHVelocity, float maxSpeed)
    {
        var scaledMoveSpeed = (MoveSpeed * Time.deltaTime * 100);
        var deltaX = scaledMoveSpeed * _moveInput.x;
        var deltaY = scaledMoveSpeed * _moveInput.y;

        localHVelocity = new Vector2(
            Mathf.Clamp(localHVelocity.x + deltaX, -maxSpeed, maxSpeed),
            Mathf.Clamp(localHVelocity.y + deltaY, -maxSpeed, maxSpeed)
            );
        if (localHVelocity.magnitude > maxSpeed) localHVelocity = localHVelocity.normalized * maxSpeed;
        return localHVelocity;
    }

    private Vector2 UpdateHVelocity(Vector2 velocityH)
    {
        float speedLimit = GetMaxSpeed();

        Vector2 worldHVelocity;

        if (_disableControls)
        {
            worldHVelocity = Vector2.zero;
            _localHVelocity = Vector2.zero;
        }
        else
        {
            _localHVelocity = UpdateLocalHVelocity(_localHVelocity, speedLimit);
            worldHVelocity = transform.Vector2ToWorldSpace(_localHVelocity);
        }

        if (IsGrounded)
        {
            _localHVelocity = _characterControllerMove.ApplyFriction(_localHVelocity);
        }
        velocityH += worldHVelocity;
        return velocityH;
    }

    //cam animation ---------------------------------------
    private float _distanceSinceStep;
    public void SetDistanceSinceStep(float f) { _distanceSinceStep = f; }
    private GameObject _cameraGO;
    private void UpdateCameraAnim(float stepProgress)
    {
        var camPosition = _cameraGO.transform.localPosition;
        camPosition.y = _playerMovementProfile.cameraAnimCurve.Evaluate(stepProgress);
        _cameraGO.transform.localPosition = camPosition;
    }

    private void Awake()
    {
        _playerSwim = GetComponent<PlayerSwim>();
        _playerAudio = GetComponent<PlayerAudio>();
    }
    private void OnEnable()
    {
        //_playerInputActionReference.action.Link(OnMove);
        //_playerInputActionReference.action.actionMap.Enable();
        _playerInput.onControlsChanged += test;
        _playerInput.actions.Link("Move", OnMove);
        _playerInput.actions.Link("Sprint", OnSprintInput);
        //_playerInput.actions.Link("Walk", OnWalkInput);
        _moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
        if (_characterControllerMove == null) _characterControllerMove = GetComponent<CharacterControllerMove>();
        _characterControllerMove.HorizontalMovementFuncs.AddUnique(new(UpdateHVelocity, GetType().Name));
    }
    public void test(PlayerInput pi)
    {
        Debug.LogError($"changed to {pi.currentControlScheme}");
    }
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= test;
        _playerInput.actions.UnLink("Move", OnMove);
        _playerInput.actions.UnLink("Sprint", OnSprintInput);
        //_playerInput.actions.UnLink("Walk", OnWalkInput);
        //input

        _characterControllerMove.HorizontalMovementFuncs.RemoveGameFunc(GetType().Name);
    }

    private void Start()
    {
        _cameraGO = GetComponent<PlayerLook>().GetCamera().gameObject;
        instance = this;

    }

    public static event Action<string> OnPlayerStep;
    private void Update()
    {
        if (IsGrounded)
        {
            Vector3 oldPosition = _characterControllerMove.PositionOnPreviousFrame;
            if (!_characterControllerMove.IsSliding() && _characterControllerMove.GroundBelow()) { PlayerData.LastGroundLocation = oldPosition; }
            _distanceSinceStep += Vector2.Distance(new(oldPosition.x, oldPosition.z), new(transform.position.x, transform.position.z));
        }
        float stepProgress = _distanceSinceStep / StepInterval;

        UpdateCameraAnim(stepProgress);

        if (_distanceSinceStep > StepInterval)
        {
            _distanceSinceStep = 0f;
            OnPlayerStep?.Invoke(_characterControllerMove.SurfaceBelowTag);
        }
    }
}
