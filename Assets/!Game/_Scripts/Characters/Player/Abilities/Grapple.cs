using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterControllerMove))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Interact))]
public class Grapple : PlayerComponent, ILookSelectorListener
{
    [SerializeField] private GrappleProfile _grappleProfile;
    public override void SetComponentProfile(ComponentProfile grp) { _grappleProfile = (GrappleProfile)grp; }

    private PlayerMove playerMove;
    private PlayerJump playerJump;
    private CharacterController controller;
    private CharacterControllerMove _characterControllerMove;
    private Vector3 anchorPosition = Vector3.zero;
    private float ropeLength;
    private LineRenderer lineRenderer;
    private bool isSwinging;
    private GameObject fulcrum = null;
    private Vector3 fulcrumDirection;
    private float fulcrumSpeed = 0f;
    private Vector3 tangentialVelocity;

    private bool isPastNadir;

    private CollisionFlags collisionFlags;
    private bool isColliding = false;
    private Vector3 previousPosition;
    private AudioSource audioSource;
    private AudioSource launchAudioSource;
    private PlayerLookSelector _playerLookSelector;

    private GrapplePoint _grapplePoint;

    private void OnEnable()
    {
        PlayerStateManager.OnStateChanged += OnStateChanged;
        _playerLookSelector.OnTriggerHit += OnRaycastTrigger;
        PlayerLookSelector.PrimaryInputListenerList.AddListener(this);
    }
    private void OnDisable()
    {
        PlayerStateManager.OnStateChanged -= OnStateChanged;
        _playerLookSelector.OnTriggerHit -= OnRaycastTrigger;
        PlayerLookSelector.PrimaryInputListenerList.RemoveListener(this);
    }
    private void OnStateChanged(PlayerState state)
    {
        if (_grapplePoint != null && state != PlayerStates.Default && state != PlayerStates.Menu) { Detach(); }
    }
    private void Detach()
    {
        if (audioSource.clip == _grappleProfile.grappleSwingSFX.Clip)
        {
            audioSource.PlaySFX(_grappleProfile.grappleRetractSFX);
        }
        lineRenderer.enabled = false;
        if (isSwinging)
        {
            //account for distortions introduced in CharacterControllerMove
            tangentialVelocity.y /= 50f;
            tangentialVelocity.x *= 7f / 2f;
            tangentialVelocity.z *= 7f / 2f;
            _characterControllerMove.SetVelocity(tangentialVelocity, false);
        }
        else { _characterControllerMove.SetVelocity(Vector3.zero); }
        anchorPosition = Vector3.zero;
        ResetFulcrum();
        _grapplePoint = null;
    }

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerMove = gameObject.GetComponent<PlayerMove>();
        lineRenderer = gameObject.GetOrAddComponent<LineRenderer>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        playerJump = GetComponent<PlayerJump>();

        launchAudioSource = gameObject.AddComponent<AudioSource>();
        launchAudioSource.playOnAwake = false;

        _characterControllerMove = GetComponent<CharacterControllerMove>();
        _playerLookSelector = GetComponent<PlayerLookSelector>();

    }
    private void OnDestroy()
    {
        Destroy(lineRenderer);
        Destroy(audioSource);
        Destroy(launchAudioSource);
    }
    private void Start()
    {
        if (_grappleProfile == null)
        {
            Debug.LogError(gameObject.name + ": No GrappleProfile set!");
            enabled = false;
        }
        audioSource.clip = _grappleProfile.grappleLaunchSFX.Clip;
        lineRenderer.widthMultiplier = _grappleProfile.ropeThickness;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = _grappleProfile.ropeMaterial;
        lineRenderer.enabled = false;
    }

    private void OnRaycastTrigger(RaycastHit hit)
    {
        if (anchorPosition != Vector3.zero) return;
        bool inState = PlayerStateManager.State == PlayerStates.Default || PlayerStateManager.State == PlayerStates.Scanner;
        if (inState && hit.TryGetComponent(out GrapplePoint grpnt, _grappleProfile.distanceLimit, _grappleProfile.layerMask) && grpnt.enabled)
        {
            if (_grapplePoint != grpnt)
            {
                _grapplePoint = grpnt;
                grpnt.Select();
                OnHover?.Invoke(_grappleProfile.grappleActionIcon);
            }
        }
        else if (_grapplePoint != null && !lineRenderer.enabled)
        {
            _grapplePoint.Deselect();
            OnStopHover?.Invoke();
            _grapplePoint = null;
        }
    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
    public bool ValidateInputAction() => _grapplePoint != null;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            StartCoroutine(Co_GrappleAttach());
            IEnumerator Co_GrappleAttach()
            {
                yield return new WaitUntil(() => _grapplePoint != null);
                launchAudioSource.PlaySFX(_grappleProfile.grappleLaunchSFX);
                lineRenderer.enabled = true;
                lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
                _grapplePoint.Deselect();
                _grapplePoint.DisableIconGO();
                StartCoroutine(Co_LineRenderer());

                OnStopHover?.Invoke();
                _grapplePoint.Effect();
                if (_grapplePoint.Loose) yield break;
                anchorPosition = _grapplePoint.transform.position;
                ropeLength = Vector3.Distance(anchorPosition, transform.position);


                if (_characterControllerMove.IsGrounded)
                    _characterControllerMove.AddVelocity(Vector3.up * 18f / 50f);

                IEnumerator Co_LineRenderer()
                {
                    yield return ExtensionMethods.Co_FadeFloat(0.1f, Vector2.up, (fl) =>
                    {
                        lineRenderer.SetPosition(1, Vector3.Lerp(transform.position, _grapplePoint.transform.position, fl));
                    });
                    while (lineRenderer.enabled)
                    {
                        lineRenderer.SetPositions(new Vector3[] { transform.position, _grapplePoint.transform.position });
                        yield return null;
                    }
                }
            }
        }
        if (ctx.canceled)
        {
            StopAllCoroutines();
            if (_grapplePoint == null) return;
            _grapplePoint.EnableIconGO();
            _grapplePoint.Release();
            Detach();
        }

    }

    private void ResetFulcrum()
    {
        playerMove.SetDisableControls(false);
        playerJump.SetDisableGravity(false);
        Destroy(fulcrum);
        isSwinging = false;
        fulcrum = null;
    }

    private void Update()
    {
        if (anchorPosition == Vector3.zero) return;

        bool hittingSides = (collisionFlags & CollisionFlags.CollidedSides) != 0;
        if (hittingSides && isSwinging && !isColliding)
        {
            fulcrumSpeed = 0f;
            isColliding = true;
        }
        else if (!hittingSides && isColliding)
        {
            isColliding = false;
        }

        if (!isSwinging)
        {
            if (Vector3.Distance(anchorPosition, transform.position) > ropeLength)
            {
                SwingInit();
            }
            if (_grapplePoint.MinimumRopeLength != 0)
                ropeLength = Mathf.Max(Vector3.Distance(anchorPosition, transform.position), _grapplePoint.MinimumRopeLength);
            else { ropeLength = Vector3.Distance(anchorPosition, transform.position); }
            return;
        }

        if (controller.isGrounded) { ResetFulcrum(); return; }

        SwingUpdate();
    }
    private void SwingInit()
    {
        if (isSwinging) return;
        Vector3 pointC = (ropeLength * Vector3.Normalize(transform.position - anchorPosition)) + anchorPosition;

        var playerVelocity = _characterControllerMove.Velocity;
        fulcrumDirection = new Vector3(transform.position.x, anchorPosition.y, transform.position.z) - anchorPosition;


        playerMove.SetDisableControls(true);
        if (!controller.isGrounded) { playerJump.SetDisableGravity(true); }

        isColliding = false;


        controller.Move(pointC - transform.position);

        fulcrum = new GameObject("GrappleFulcrum");
        Vector3 direction = transform.position - anchorPosition;
        fulcrum.transform.SetPositionAndRotation(anchorPosition, Quaternion.LookRotation(direction));
        isSwinging = true;
        if (audioSource.clip != _grappleProfile.grappleSwingSFX.Clip)
        {
            audioSource.PlaySFX(_grappleProfile.grappleSwingSFX);
            audioSource.volume = 0;
        }
        // below code is too inaccurate to calculate launch velocity for swing start
        //fulcrumVelocity = Vector3.Distance(_characterControllerMove.Velocity, Vector3.zero) * 0.8f / (GrappleProfile.VelocityConversionRate * _grappleProfile.GrappleSpeedProfile.Evaluate((ropeLength / 13f)));
        var ropeDirection = Vector3.Normalize(transform.position - anchorPosition);
        var tanDirection = Vector3.Cross(ropeDirection, fulcrum.transform.right).normalized;
        //player velocity converted to tangential swing velocity, and then to angular velocity around fulcrum
        // tangential velocity is dot product of player velocity and tangent direction of swing circle at player position
        // think of it like this: the player is moving in a straight line, but the rope forces them to move in a circle around the anchor point
        float tanVel = Vector3.Dot(playerVelocity.normalized, tanDirection) * playerVelocity.magnitude;

        //convert tangential velocity to angular velocity
        // to get angular velocity, divide tangential velocity by radius (rope length), and convert to degrees per second
        fulcrumSpeed = (tanVel / ropeLength) * Mathf.Rad2Deg;
    }
    private void SwingUpdate()
    {
        float angleBetween = Vector3.Angle(fulcrumDirection, fulcrum.transform.forward);

        isPastNadir = (angleBetween > 90 && angleBetween < 180);
        bool swingBackward = isPastNadir && fulcrumSpeed > -GrappleProfile.SwingSpeedLimit;
        bool swingForward = !isPastNadir && fulcrumSpeed < GrappleProfile.SwingSpeedLimit;
        if (swingBackward)
        {
            fulcrumSpeed -= GrappleProfile.swingSpeedAmt * Time.deltaTime * 0.5f;
        }
        else if (swingForward)
        {
            fulcrumSpeed += GrappleProfile.swingSpeedAmt * Time.deltaTime * 0.5f;
        }

        float scaledFulcrumSpeed = fulcrumSpeed * Time.deltaTime;
        fulcrum.transform.Rotate(Vector3.right, scaledFulcrumSpeed, Space.Self);
        var tanDirection = (transform.position - previousPosition).normalized;

        //convert rotational speed to tangential speed
        var tanMagnitude = Math.Abs(scaledFulcrumSpeed * Mathf.Deg2Rad) * ropeLength;
        //apply to tangential velocity vector
        tangentialVelocity = tanDirection * tanMagnitude;

        //this is framerate independent because only half the acceleration is applied when we calculate tangential velocity
        //this works because when we detach, we are no longer accelerating within that frame, meaning only half of the velocity need apply


        if (swingBackward)
        {
            fulcrumSpeed -= GrappleProfile.swingSpeedAmt * Time.deltaTime * 0.5f;
        }
        else if (swingForward)
        {
            fulcrumSpeed += GrappleProfile.swingSpeedAmt * Time.deltaTime * 0.5f;
        }
        Vector3 pointC = (ropeLength * fulcrum.transform.forward) + fulcrum.transform.position;






        previousPosition = transform.position;
        collisionFlags = controller.Move(pointC - transform.position);




        float targetVolume = _grappleProfile.grappleSwingSFX.Volume * (Mathf.Abs(fulcrumSpeed) / GrappleProfile.SwingSpeedLimit);
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, _grappleProfile.swingAudioCurve.Evaluate(targetVolume), Time.deltaTime);
    }
}
