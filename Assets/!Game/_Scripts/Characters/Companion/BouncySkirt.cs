using UnityEngine;

[RequireComponent(typeof(CompanionBehaviorManager))]
public class BouncySkirt : CompanionComponent
{

    [SerializeField] private BouncySkirtProfile bouncySkirtProfile;
    private AudioSource audioSource;

    private CompanionBehaviorManager _companionBehaviorManager;

    private bool hasUpgrade = false;

    private bool hasBouncedOnce = false;

    public override void SetComponentProfile(ComponentProfile profile)
    {
        bouncySkirtProfile = (BouncySkirtProfile)profile;
    }
    private void OnEnable()
    {
        PlayerJump.OnLand += OnPlayerLand;
        Inventory.OnInventoryChange += OnInventoryChange;
    }

    private void OnInventoryChange(Inventory inventory, InventoryItem item, bool enabled)
    {
        if (inventory != Inventories.Instance.CompanionInventory) return;
        if (item.Metadata != bouncySkirtProfile.BouncyUpgradeItem) return;
        hasUpgrade = enabled;
    }

    private void OnPlayerLand() { hasBouncedOnce = false; }
    private void OnDisable()
    {
        PlayerJump.OnLand -= OnPlayerLand;
        Inventory.OnInventoryChange -= OnInventoryChange;
    }

    protected override void Awake()
    {
        base.Awake();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        _companionBehaviorManager = GetComponent<CompanionBehaviorManager>();
    }
    private void Start()
    {
        CheckForUpgrade();
    }

    private void CheckForUpgrade()
    {
        hasUpgrade = Inventories.Instance.CompanionInventory.Has(bouncySkirtProfile.BouncyUpgradeItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_companionBehaviorManager == null) return;
        if (
            _companionBehaviorManager.CurrentBehavior == CompanionManager.CompanionBehaviors.FearBehavior ||
            _companionBehaviorManager.CurrentBehavior == CompanionManager.CompanionBehaviors.SitBehavior
            ) return;
        if (_companionBehaviorManager.Companion.CarriedObject != null) return;
        if (other.gameObject.tag == PlayerManager.Tag)
        {

            if (!other.TryGetComponent(out CharacterControllerMove playerMove)) return;

            Vector3 velocity = playerMove.Velocity;
            if (velocity.y > bouncySkirtProfile.VelocityThreshold) return;
            if (!hasUpgrade && hasBouncedOnce)
            {
                velocity.y *= 0.8f;
            }
            playerMove.SetVelocity(new Vector3(velocity.x, -velocity.y, velocity.z), playerMove.AirControl);
            hasBouncedOnce = true;
            //bouncySkirtProfile.OnSkirtBounceGE.Raise();
            _companionBehaviorManager.SetAnimatorParam.SetTrigger("Bounce");
            audioSource.PlaySFX(bouncySkirtProfile.BounceSFX);

        }
        else if (other.TryGetComponent(out IBounceable bounceable))
        {
            if (!bounceable.CanBounce()) return;
            bounceable.Bounce();
            _companionBehaviorManager.SetAnimatorParam.SetTrigger("Bounce");
            audioSource.PlaySFX(bouncySkirtProfile.BounceSFX);
        }
    }
}
