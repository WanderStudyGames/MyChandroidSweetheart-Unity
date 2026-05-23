using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(CharacterControllerMove))]
[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
public class Sprocket : PlayerComponent
{
    [SerializeField] private SprocketProfile sprocketProfile;
    private PlayerInput _playerInput;
    public override void SetComponentProfile(ComponentProfile spp) { sprocketProfile = (SprocketProfile)spp; }

    private CharacterControllerMove _characterControllerMove;
    private PlayerMove _playerMove;
    private float launchMultiplier = 0f;
    private float pressureDistance;
    private bool sprocketing;
    private bool canSprocket;
    private bool hasJumpItem;
    private RaycastHit castHit;
    private SFX _chargeSfx;

    private AudioSource audioSource;
    private AudioSource chargeAudioSource;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<PlayerLook>().GetCamera();
        _characterControllerMove = gameObject.GetComponent<CharacterControllerMove>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        chargeAudioSource = gameObject.AddComponent<AudioSource>();
        chargeAudioSource.playOnAwake = false;

        _playerMove = GetComponent<PlayerMove>();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        CheckJumpItem();
    }
    public static event Action OnDestroyPlayer;
    private void OnDestroy()
    {
        Destroy(audioSource);
        Destroy(chargeAudioSource);
        OnDestroyPlayer?.Invoke();

    }
    private void OnEnable()
    {
        _playerInput.actions.Link("Tool", OnSprocket);
        PlayerStateManager.OnStateChanged += OnStateChanged;
        Inventory.OnInventoryChange += OnInventoryChange;
    }
    private void OnInventoryChange(Inventory inventory, InventoryItem item, bool b)
    {
        if (item.Metadata == sprocketProfile.sprocketJumpItem && inventory == Inventories.Instance.PlayerInventory)
        {
            hasJumpItem = b;
            _chargeSfx = b ? sprocketProfile.ChargeJumpSFX : sprocketProfile.ChargeSFX;
        }
    }
    private void CheckJumpItem()
    {
        hasJumpItem = Inventories.Instance.PlayerInventory.Has(sprocketProfile.sprocketJumpItem);
        _chargeSfx = hasJumpItem ? sprocketProfile.ChargeJumpSFX : sprocketProfile.ChargeSFX;
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Tool", OnSprocket);
        PlayerStateManager.OnStateChanged -= OnStateChanged;
        Inventory.OnInventoryChange -= OnInventoryChange;
    }

    private void OnStateChanged(PlayerState state)
    {
        if (state != PlayerStates.Default && state != PlayerStates.Menu) SprocketLaunch();
    }

    private void SprocketCharge()
    {
        chargeAudioSource.PlaySFX(_chargeSfx);

        sprocketing = true;
        launchMultiplier = 0.01f;
        PlayerData.SprocketCharge = launchMultiplier;

        _playerMove.SpeedMultipliers.AddUnique(new(sprocketProfile.sprocketingWalkSpeedMultiplier, GetType().Name));
        OnSprocketCharge?.Invoke();
    }
    public static event Action OnSprocketCharge;

    private void SprocketLaunch()
    {
        if (!sprocketing) return;
        sprocketing = false;
        if (canSprocket && castHit.collider != null)
        {

            bool canLaunch = true;
            bool playedFX = false;
            Vector3 force = _camera.transform.forward.normalized * launchMultiplier * sprocketProfile.PressureProfile.Evaluate(pressureDistance);
            float magnitude = force.magnitude;
            foreach (ISprocketPushable pushable in castHit.collider.gameObject.GetComponents<ISprocketPushable>())
            {
                bool b = pushable.Push(force);
                if (!b) canLaunch = false;
                Debug.Log(pushable.GetType().Name + " sprocket pushed with force magnitude: " + force.magnitude);

                //fx
                if (magnitude < sprocketProfile.littleChargeCutoff || playedFX) continue;
                ParticleSystem ps = sprocketProfile.impactParticlesSmall;
                SFX sfx = sprocketProfile.ImpactSFXSmall;
                if (magnitude > sprocketProfile.bigChargeCutoff)
                {
                    ps = sprocketProfile.impactParticlesBig;
                    sfx = sprocketProfile.ImpactSFXBig;
                }
                Instantiate(ps, castHit.point, Quaternion.LookRotation(castHit.normal));
                SFX.PlayAtPoint(sfx, castHit.point);
                playedFX = true;
                //end fx

            }
            if (castHit.collider.gameObject.layer == LayerMask.NameToLayer(sprocketProfile.physicsLayer))
            {
                if (castHit.collider.gameObject.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddForce(launchMultiplier * sprocketProfile.maxLaunchForce * sprocketProfile.physicsPushMultiplier * sprocketProfile.PressureProfile.Evaluate(pressureDistance) * Vector3.Normalize(_camera.transform.forward), ForceMode.Impulse);
                }
            }
            //SHOOT PLAYER BACKWARD
            else if (hasJumpItem)
            {
                if (_characterControllerMove.Velocity.y > 0) { _characterControllerMove.SetVelocity(Vector3.zero); }
                if (canLaunch)
                {
                    _characterControllerMove.AddVelocity(launchMultiplier * sprocketProfile.maxLaunchForce * sprocketProfile.PressureProfile.Evaluate(pressureDistance) * -_camera.transform.forward);
                    //fx
                    if (magnitude > sprocketProfile.bigChargeCutoff)
                    {
                        SFX.PlayAtPoint(sprocketProfile.SprocketLaunchJumpBig, transform.position);
                        SFX.PlayAtPoint(sprocketProfile.ImpactSFXBig, castHit.point);
                        Instantiate(sprocketProfile.impactParticlesBig, castHit.point, Quaternion.identity);
                    }
                    else if (magnitude > sprocketProfile.littleChargeCutoff)
                    {
                        SFX.PlayAtPoint(sprocketProfile.SprocketLaunchJumpSmall, transform.position);
                        SFX.PlayAtPoint(sprocketProfile.ImpactSFXSmall, castHit.point);
                        Instantiate(sprocketProfile.impactParticlesSmall, castHit.point, Quaternion.identity);
                    }

                }
                canSprocket = false;
            }



        }
        chargeAudioSource.Stop();
        if (PlayerData.SprocketCharge > sprocketProfile.bigChargeCutoff)
        {
            audioSource.PlaySFX(sprocketProfile.BigLaunchSFX);
        }
        else if (PlayerData.SprocketCharge > sprocketProfile.littleChargeCutoff)
        {
            audioSource.PlaySFX(sprocketProfile.LittleLaunchSFX);
        }
        else { audioSource.PlaySFX(sprocketProfile.FailSFX); }

        _playerMove.SpeedMultipliers.RemoveByName(GetType().Name);
        OnSprocketRelease?.Invoke();
        launchMultiplier = 0;
        PlayerData.SprocketCharge = 0;
    }
    public static event Action OnSprocketRelease;
    public void OnSprocket(InputAction.CallbackContext context)
    {
        if (PlayerStateManager.State != PlayerStates.Default) return;
        if (enabled)
        {
            if (context.performed)
            {
                SprocketCharge();
            }
            if (context.action.WasReleasedThisFrame())
            {
                Debug.Log($"Player State: {PlayerStateManager.State}");
                SprocketLaunch();
            }
        }
    }

    void UpdateDistance()
    {
        Physics.Raycast(_camera.transform.position, _camera.transform.forward, out castHit, 1000f, sprocketProfile.layerMask);
        if (castHit.distance == 0) pressureDistance = 100f; else pressureDistance = castHit.distance;
        PlayerData.SprocketDistance = sprocketProfile.PressureProfile.Evaluate(pressureDistance);
    }

    private void Update()
    {
        if (sprocketing)
        {
            launchMultiplier = Mathf.Clamp(launchMultiplier + (sprocketProfile.chargeSpeed * Time.deltaTime * 100), 0, 1);
            PlayerData.SprocketCharge = launchMultiplier;
        }

        UpdateDistance();

        if (_characterControllerMove.IsGrounded) { canSprocket = true; }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new(transform.position.x, transform.position.y + 4.7f, transform.position.z));
    }
}
