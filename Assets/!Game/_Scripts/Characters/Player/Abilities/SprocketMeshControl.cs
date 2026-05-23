using System;
using System.Collections;

using UnityEngine;

public class SprocketMeshControl : MonoBehaviour
{
    [Dependency][SerializeField] private GameObject pressurizerGO;
    [Dependency][SerializeField] private Animator sprocketAnimator;
    [Dependency][SerializeField] private ParticleSystem airDensityPS;
    [Dependency][SerializeField] private ParticleSystem shotBigPS;
    [Dependency][SerializeField] private ParticleSystem shotLittlePS;
    [Dependency][SerializeField] private MeshRenderer indicatorRenderer;
    [Dependency][SerializeField] private SprocketProfile sprocketProfile;
    [Dependency][SerializeField] private Transform handUpPosition;
    [Dependency][SerializeField] private Transform handDownPosition;
    [Dependency][SerializeField] private LerpToTransform ltt;
    [Dependency][SerializeField] private Jitter jitter;
    [Dependency][SerializeField] private GameObject meshObject;
    [SerializeField] private Gradient indicatorGradient;
    [SerializeField] private AnimationCurve pressurizerCurve;
    [SerializeField] private AnimationCurve particleCurve;
    private float defaultPressurizerYScale;
    private Transform _parent;
    bool _hasSprocket;
    bool _hasSprocketJump;
    private bool _chargedSprocket = false;
    private void Awake()
    {
        _parent = transform.parent;
        defaultPressurizerYScale = pressurizerGO.transform.localScale.y;
        airDensityPS.gameObject.SetActive(false);
        indicatorRenderer.material.SetColor("_Emissive_Color", indicatorGradient.colorKeys[0].color);
        jitter.mix = 0;
    }
    void Detach()
    {
        if (_chargedSprocket)
        {
            transform.parent = null;
            _chargedSprocket = false;
        }
    }
    void Attach() { transform.parent = _parent; }

    void PutHandDown()
    {
        ltt.SetTarget(handDownPosition);
    }
    void OnStateChanged(PlayerState state)
    {
        if (state == PlayerStates.Tablet) { ltt.SetTarget(null); sprocketAnimator.gameObject.SetActive(false); }
        else if (state != PlayerStates.Default && state != PlayerStates.Menu) { ltt.SetTarget(PlayerLook.Camera.transform); PutHandDown(); sprocketAnimator.gameObject.SetActive(_hasSprocket); }
        else { sprocketAnimator.gameObject.SetActive(_hasSprocket); if (_hasSprocketJump) PutHandUp(); }
    }
    void OnSetAbility(Type T, bool b)
    {
        if (T != typeof(Sprocket)) return;
        _hasSprocket = b;
        if (b)
        {
            PutHandUp();
        }
        else
        {
            PutHandDown();
        }
        meshObject.SetActive(b);
    }
    void OnInventoryChange(Inventory inventory, InventoryItem item, bool has)
    {
        if (inventory == Inventories.Instance.PlayerInventory && item.Metadata.name == "Sprocket Gun Jump")
        {
            _hasSprocketJump = true;
            if (has)
            {
                PutHandUp();
            }
        }
    }
    void PutHandUp()
    {
        if (_hasSprocket)
            ltt.SetTarget(handUpPosition);
    }
    private void OnDestroyPlayer()
    {
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        PlayerStateManager.OnStateChanged += OnStateChanged;
        Sprocket.OnSprocketCharge += ChargeStart;
        Sprocket.OnSprocketRelease += ChargeRelease;
        PlayerAbilityManager.OnSetAbility += OnSetAbility;
        Inventory.OnInventoryChange += OnInventoryChange;
        PlayerJump.OnLand += Attach;
        PlayerJump.OnLeaveGround += Detach;

        ltt.enabled = false;
        ltt.transform.SetPositionAndRotation(handDownPosition.position, handDownPosition.rotation);
        ltt.SetTarget(handDownPosition);

        StartCoroutine(OneFrame());
        IEnumerator OneFrame()
        {
            yield return null;
            Sprocket.OnDestroyPlayer += OnDestroyPlayer;
            ltt.enabled = true;
            PutHandUp();
        }
    }
    private void OnDisable()
    {
        PlayerStateManager.OnStateChanged -= OnStateChanged;
        Sprocket.OnSprocketCharge -= ChargeStart;
        Sprocket.OnSprocketRelease -= ChargeRelease;
        Inventory.OnInventoryChange -= OnInventoryChange;
        PlayerAbilityManager.OnSetAbility -= OnSetAbility;
        PlayerJump.OnLand -= Attach;
        PlayerJump.OnLeaveGround -= Detach;
        Sprocket.OnDestroyPlayer -= OnDestroyPlayer;
    }
    private void Start()
    {
        _hasSprocket = PlayerAbilityManager.HasAbility(typeof(Sprocket));
        _hasSprocketJump = true;// Inventories.Instance.PlayerInventory.Has("Sprocket Gun Power Pack");
        meshObject.SetActive(_hasSprocket);
        if (_hasSprocketJump) return;
        PutHandDown();

    }
    void ChargeStart()
    {
        ltt.SetTarget(handUpPosition);
        StopAllCoroutines();
        PutHandUp();
        StartCoroutine(Charge());
        sprocketAnimator.SetBool("Charge", true);
        sprocketAnimator.enabled = true;
        airDensityPS.gameObject.SetActive(true);
    }

    public IEnumerator Charge()
    {
        _chargedSprocket = true;
        jitter.StartJitter(PlayerData.SprocketCharge);
        while (PlayerData.SprocketCharge > 0)
        {
            pressurizerGO.transform.localScale =
            new(
            pressurizerGO.transform.localScale.x,
            pressurizerCurve.Evaluate(PlayerData.SprocketCharge),
            pressurizerGO.transform.localScale.z
            );

            Color color =
                new(
                    airDensityPS.main.startColor.color.r,
                    airDensityPS.main.startColor.color.g,
                    airDensityPS.main.startColor.color.b,
                    particleCurve.Evaluate(PlayerData.SprocketCharge)
                );
            indicatorRenderer.material.SetColor("_Emissive_Color", indicatorGradient.Evaluate(PlayerData.SprocketDistance * PlayerData.SprocketCharge));
            airDensityPS.SetStartColor(color);

            jitter.mix = PlayerData.SprocketCharge;

            sprocketAnimator.enabled = PlayerData.SprocketCharge < 1;

            yield return null;
        }
        jitter.mix = 0;

    }

    void ChargeRelease()
    {


        pressurizerGO.transform.localScale = new(
            pressurizerGO.transform.localScale.x,
            defaultPressurizerYScale,
            pressurizerGO.transform.localScale.z);
        sprocketAnimator.enabled = false;
        sprocketAnimator.SetBool("Charge", false);
        airDensityPS.gameObject.SetActive(false);
        indicatorRenderer.material.SetColor("_Emissive_Color", indicatorGradient.colorKeys[0].color);
        jitter.mix = 0;

        if (PlayerData.SprocketCharge > sprocketProfile.bigChargeCutoff)//0.66f
        {
            shotBigPS.Play();
        }
        else if (PlayerData.SprocketCharge > sprocketProfile.littleChargeCutoff)//0.15f
        {
            shotLittlePS.Play();
        }
        _chargedSprocket = true;
        if (_hasSprocketJump) return;
        IEnumerator HandDown()
        {
            yield return new WaitForSeconds(10f);
            PutHandDown();
        }
        StartCoroutine(HandDown());
    }
}
