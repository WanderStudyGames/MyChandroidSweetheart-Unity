using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterControllerMove))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerSwim))]
[RequireComponent(typeof(PlayerAudio))]
public class PlayerJump : MonoBehaviour
{
    private PlayerSwim _playerSwim;
    private PlayerAudio _playerAudio;
    private PlayerMove _playerMove;
    private bool IsSwimming => _playerSwim.IsSwimming;
    [SerializeField] private PlayerMovementProfile _playerMovementProfile;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Jitter _jitter;
    [SerializeField] private Animator _splashAnimator;
    private CharacterControllerMove _characterControllerMove;

    private bool IsGrounded => _characterControllerMove.IsGrounded;
    [SerializeField, ReadOnly]
    private bool _inAir = false;
    private bool _hasFallDamageItemPrimary;
    private bool _hasFallDamageItemSecondary;

    private void Awake()
    {
        _playerSwim = GetComponent<PlayerSwim>();
        _playerAudio = GetComponent<PlayerAudio>();
        _playerMove = GetComponent<PlayerMove>();
        _characterControllerMove = GetComponent<CharacterControllerMove>();
        _hasFallDamageItemPrimary = Inventories.Instance.PlayerInventory.Has(_playerMovementProfile.PreventFallDamageItem);
        _hasFallDamageItemSecondary = Inventories.Instance.PlayerInventory.Has(_playerMovementProfile.PreventFallDamageItemSecondary);

    }
    private void Start()
    {
        _characterControllerMove.Move(new(0, -0.1f, 0));
        _inAir = false;
    }
    private void OnEnable()
    {
        _playerInput.actions.Link("Jump", OnJump);
        _characterControllerMove.VerticalMovementFuncs.AddUnique(new(UpdateVerticalVelocity, GetType().Name));
        _inAir = false;
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Jump", OnJump);
        _characterControllerMove.VerticalMovementFuncs.RemoveGameFunc(GetType().Name);

        _playerAudio.StopFalling();
        _playerMove.SetDistanceSinceStep(0);
        _inAir = false;

    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame()) { Jump(); }
    }
    private float UpdateVerticalVelocity(float velocityY)
    {
        var delta = 0f;
        if (!_disableGravity)
            delta -= _playerMovementProfile.gravity * Time.deltaTime;
        return delta;
    }

    [SerializeField] private bool _disableGravity = false;
    public void SetDisableGravity(bool b)
    {
        _disableGravity = b;
        _characterControllerMove.SetGravityEnabled(!b);
        if (b)
        {
            var velocity = _characterControllerMove.Velocity;
            velocity.y = 0;
            _characterControllerMove.SetVelocity(velocity);
        }
    }
    private bool IsSliding()
    {
        return _characterControllerMove.IsSliding();
    }
    private int _comboedJumps = 0;
    public void Jump()
    {
        if (IsGrounded && !IsSliding())
        {
            if (_jumpComboActive)
            {
                var aSource = _playerMovementProfile.jumpComboSFX.PlayAtPoint(transform.position);
                _comboedJumps++;
                aSource.pitch = Mathf.Lerp(1f, 1.5f, _comboedJumps / 4f);
                aSource.volume = Mathf.Lerp(_playerMovementProfile.jumpComboSFX.Volume / 1.5f, _playerMovementProfile.jumpComboSFX.Volume, _comboedJumps / 4f);
            }
            else { _comboedJumps = 0; }
            _inAir = false;
            var velocity = _characterControllerMove.Velocity;
            velocity.y = Mathf.Lerp(_playerMovementProfile.jumpForce, _playerMovementProfile.jumpForce * 1.5f, _comboedJumps / 4);
            _characterControllerMove.SetVelocity(velocity);

            _characterControllerMove.Move(new Vector3(0f, 0.1f, 0f));

            if (IsSwimming) { _playerMovementProfile.OnExitWaterGE.Raise(); }
            if (!_jumpComboActive)
            {
                _playerAudio.Jump(_characterControllerMove.SurfaceBelowTag, true);
                var aSource = _playerMovementProfile.jumpComboSFX.PlayAtPoint(transform.position);
                aSource.pitch = 1f;
                aSource.volume = _playerMovementProfile.jumpComboSFX.Volume / 2f;
            }
        }
    }
    public static event Action OnLeaveGround;
    public static event Action OnLand;
    public static event Action<float> OnLandWater;
    private bool _jumpComboActive = false;
    IEnumerator Co_WaitForLand()
    {
        OnLeaveGround?.Invoke();
        var initialYPosition = transform.position.y;
        var fallingAudioSource = _playerAudio.StartFalling();
        var volume = _playerMovementProfile.fallingSFX.Volume;
        Vector3 ctrlVel = Vector3.zero;
        AnimationCurve animCurve = _playerMovementProfile.fallingVolumeAnimCurve;
        var time = 0f;
        while (!IsGrounded || IsSliding())
        {
            time += Time.deltaTime;
            if (_disableGravity) yield return null;
            ctrlVel = _characterControllerMove.Velocity;
            if (ctrlVel.y < 0f)
            {
                fallingAudioSource.volume = volume * animCurve.Evaluate(ctrlVel.magnitude);
            }
            else { fallingAudioSource.volume = 0f; }
            if (time > 25f) { Debug.LogError("Falling too long!"); Debug.LogError($"Controller isGrounded: {IsGrounded}"); }
            yield return null;
        }

        StartCoroutine(Co_JumpCombo());
        IEnumerator Co_JumpCombo()
        {
            time = 0;
            _jumpComboActive = true;
            while (time < 0.04f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            _jumpComboActive = false;
        }


        _playerAudio.StopFalling();
        _playerMove.SetDistanceSinceStep(0);

        bool fallDeathPrimary =
            !_hasFallDamageItemPrimary &&
            transform.position.y - initialYPosition < -_playerMovementProfile.fallDamageThreshhold;
        bool fallDeathSecondary =
            !_hasFallDamageItemSecondary &&
            transform.position.y - initialYPosition < -_playerMovementProfile.fallDamageThreshholdSecondary;
        bool dieByFallDamage = fallDeathPrimary || fallDeathSecondary;

        if (!_playerSwim.UpdateIsSwimming() && !IsSwimming && dieByFallDamage)
        {
            //die
            _playerMove.SetDisableControls(true);
            SetDisableGravity(true);
            GlobalPlayerInput.Instance.enabled = false;
            DeathUI.SetSprite(_playerMovementProfile.fallDeathSprite);
            UIFade.FadeColor = Color.black;
            UIFade.FadeDurations = new(0, 0, 1);
            MusicManager.KillBGM();
            UIFade.ExecuteAfterFade(() =>
            {
                StartCoroutine(Co_DeathWait());

            });
            IEnumerator Co_DeathWait()
            {
                yield return new WaitForSecondsRealtime(4);
                SceneHandler.ResetScene();
                _characterControllerMove.SetVelocity(Vector3.zero);
                yield break;
            }
            DontDestroyOnLoad(_playerMovementProfile.fallDamageSFX.PlayAtPoint(transform.position));
        }
        else
        {
            _inAir = false;
            if (!_playerAudio.LandIsPlaying)
            {
                if (IsSwimming)
                {
                    _playerMovementProfile.OnEnterWaterGE.Raise();
                    OnLandWater?.Invoke(ctrlVel.y);
                }
            }
            _playerAudio.Land(_characterControllerMove.SurfaceBelowTag, ctrlVel.y);

            if (!IsSwimming && Mathf.Abs(ctrlVel.y) > _playerMovementProfile.largeFallThreshhold)
            {
                var go = Instantiate(_playerMovementProfile.longFallParticlePrefab);
                go.transform.position = transform.position;
                _jitter.StartJitterTimer(0, 0, 0.5f);
                _splashAnimator.Play("Fall");
            }
        }
        OnLand?.Invoke();

    }
    private void Update()
    {
        bool timeMoving = Time.deltaTime > 0 && Time.timeScale > 0;
        bool notCollidingWithFloor = (_characterControllerMove.CollisionFlags & CollisionFlags.Below) == 0;
        if (!_inAir && timeMoving && notCollidingWithFloor)
        {
            StopAllCoroutines();
            StartCoroutine(Co_WaitForLand());
            _inAir = true;
        }
    }
}
