using System;
using UnityEngine;
using UnityEngine.Events;

public class GrapplePoint : MonoBehaviour
{
    [Dependency][SerializeField] private GrappleProfile grp;
    [SerializeField] private GrapplePointProfile grappleObjectProfile;
    [SerializeField] private float size = 1;
    [SerializeField] private UnityEvent _onGrapple;
    [SerializeField] private UnityEvent _onRelease;
    [SerializeField] private bool _detachAndLerp;
    [SerializeField] private float _minimumRopeLength = 0f;
    public float MinimumRopeLength => _minimumRopeLength;
    public bool Loose => _detachAndLerp;
    private bool _attached = false;
    public bool Attached => _attached;
    private AudioSource audioSource;
    private AudioSource uiAudioSource;
    private GameObject grappleIconGO;
    private Animator iconAnimator;
    private bool _hasGrapple;
    private ParticleSystem _particleSystem;
    void Awake()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = false;
        }
        audioSource = gameObject.GetOrAddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        grappleIconGO = Instantiate(GrappleProfile.GrappleIconPF);
        if (_detachAndLerp)
        {
            grappleIconGO.transform.SetParent(null, false);
            grappleIconGO.transform.position = transform.position;
            grappleIconGO.transform.rotation = Quaternion.identity;
            grappleIconGO.AddComponent<LerpToTransform>().SetTarget(transform, 20f).IncludeRotation = false;
        }
        else
        {
            grappleIconGO.transform.SetParent(transform, false);
        }

        uiAudioSource = gameObject.AddComponent<AudioSource>();
        uiAudioSource.playOnAwake = false;
        iconAnimator = grappleIconGO.GetOrAddComponent<Animator>();
        var particleGO = Instantiate(grappleObjectProfile.ParticleSystemPrefab);
        particleGO.transform.SetParent(transform, false);
        particleGO.transform.localScale *= size;
        _particleSystem = particleGO.GetComponent<ParticleSystem>();
        Deselect();
        OnSetAbility(typeof(Grapple), PlayerAbilityManager.HasAbility(typeof(Grapple)));

    }
    private void Start()
    {
        if (PlayerAbilityManager.HasAbility(typeof(Grapple)))
        {
            _hasGrapple = true;
            EnableIconGO();
        }
    }
    private void OnEnable()
    {
        //Scanner.OnEnterScanMode += DisableIconGO;
        //Scanner.OnEnterDefaultMode += EnableIconGO;
        PlayerAbilityManager.OnSetAbility += OnSetAbility;
        PlayerStateManager.OnStateChanged += OnStateChanged;
        grappleIconGO?.SetActive(_hasGrapple);
        grappleIconGO.transform.position = transform.position;
        Deselect();
    }
    void OnStateChanged(PlayerState state)
    {
        if (state != PlayerStates.Default && state != PlayerStates.Menu && state != PlayerStates.Scanner) { DisableIconGO(); }
        else if (state == PlayerStates.Default || state == PlayerStates.Scanner) { EnableIconGO(); }
    }
    private void OnSetAbility(Type type, bool b)
    {
        if (type == typeof(Grapple))
        {
            grappleIconGO.SetActive(b);
            _hasGrapple = b;
        }
    }
    private void OnDisable()
    {
        //Scanner.OnEnterScanMode -= DisableIconGO;
        //Scanner.OnEnterDefaultMode -= EnableIconGO;
        PlayerAbilityManager.OnSetAbility -= OnSetAbility;
        PlayerStateManager.OnStateChanged -= OnStateChanged;
        if (grappleIconGO != null) grappleIconGO.SetActive(false);
    }
    public void EnableIconGO() { if (grappleIconGO != null) grappleIconGO?.SetActive(_hasGrapple); }
    public void DisableIconGO() { grappleIconGO?.SetActive(false); }
    public void Select()
    {
        if (grappleIconGO == null || !grappleIconGO.activeSelf) return;
        SetIconColor(GrappleProfile.GrappleIconActiveColor);
        uiAudioSource.PlaySFX(GrappleProfile.GrappleIconEngageSFX);
        iconAnimator.Play("Anim_GrappleIcon");
    }
    public void Deselect()
    {
        if (grappleIconGO == null || !grappleIconGO.activeSelf) return;
        SetIconColor(GrappleProfile.GrappleIconPassiveColor);
    }
    public void SetIconColor(Color color)
    {
        grappleIconGO.GetComponent<MeshRenderer>().material.color = color;
    }
    private void HookLandAudio()
    {
        audioSource.PlaySFX(grappleObjectProfile.HookLandSFX);
    }
    public void Effect()
    {
        if (grappleObjectProfile != null)
        {
            Invoke("HookLandAudio", 0.1f);
            if (_particleSystem != null) _particleSystem.Play();

        }
        _onGrapple.Invoke();
        _attached = true;
    }
    public void Release()
    {
        _onRelease.Invoke();
        _attached = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grp.distanceLimit);
        if (grappleObjectProfile != null)
        {
            Gizmos.color = Color.blue;
            float r = grappleObjectProfile.ParticleSystemPrefab.GetComponent<ParticleSystem>().shape.radius;
            Gizmos.DrawWireSphere(transform.position, r * size);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _minimumRopeLength);
    }
}
