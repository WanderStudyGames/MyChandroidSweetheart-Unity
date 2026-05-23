using UnityEngine;

[CreateAssetMenu(fileName = "BouncySkirtProfile", menuName = "ScriptableObjects/BouncySkirtProfile")]
[ProfileFor(typeof(BouncySkirt))]
public class BouncySkirtProfile : CompanionAbilityProfile
{
    [SerializeField] private float velocityThreshold = -0.06f;
    [SerializeField][Dependency] private GameEvent _onSkirtBounceGE;
    [SerializeField] private InventoryItemMetadata _bouncySkirtItem;
    [SerializeField] private InventoryItemMetadata _bouncyUpgradeItem;
    public InventoryItemMetadata BouncySkirtItem => _bouncySkirtItem;
    public InventoryItemMetadata BouncyUpgradeItem => _bouncyUpgradeItem;
    public GameEvent OnSkirtBounceGE => _onSkirtBounceGE;
    public float VelocityThreshold => velocityThreshold;
    [SerializeField] private SFX bounceSFX;
    public SFX BounceSFX => bounceSFX;
}