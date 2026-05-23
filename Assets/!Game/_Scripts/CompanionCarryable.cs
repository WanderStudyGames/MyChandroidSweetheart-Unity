using UnityEngine;
using UnityEngine.Events;
public class CompanionCarryable : MonoBehaviour, IAttachableCharacter
{
    [SerializeField] private UnityEvent _onCarry;
    [SerializeField] private UnityEvent _onDrop;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _requiredItem;

    private Companion _companion;
    private LerpToTransform _lerpToTransform;
    private bool hasUpgrade;
    private bool hasRequiredItem;
    private void Awake()
    {
        _lerpToTransform = gameObject.GetOrAddComponent<LerpToTransform>();
        hasUpgrade = Inventories.Instance.CompanionInventory.Has("Bronze Shoulder Reinforcements");
    }
    private void OnInventoryChange(Inventory inventory, InventoryItem item, bool enabled)
    {
        if (inventory == Inventories.Instance.CompanionInventory && item.Metadata.Name == "Bronze Shoulder Reinforcements")
        {
            hasUpgrade = enabled;
        }
    }
    private void OnEnable()
    {
        Inventory.OnInventoryChange += OnInventoryChange;
    }
    private void OnDisable()
    {
        Inventory.OnInventoryChange -= OnInventoryChange;
    }
    private void Start()
    {

        hasRequiredItem = _requiredItem == null || _inventory == null || _inventory.Has(_requiredItem);
    }
    public void Carry(Companion companion)
    {
        if (!hasRequiredItem || !hasUpgrade || companion.CarriedObject != null) return;
        _companion = companion;
        companion.CarriedObject = this;
        _lerpToTransform.IncludeRotation = false;
        _lerpToTransform.enabled = true;
        _lerpToTransform.SetTarget(companion.CarryingPosition, 20f);
        _onCarry.Invoke();
    }
    public void Drop(Vector3 position)
    {
        _lerpToTransform.SetTarget(null);
        _lerpToTransform.enabled = false;
        transform.position = position;
        _companion.CarriedObject = null;
        _onDrop.Invoke();
    }

    public void AttachTo(GameObject go)
    {
        transform.SetParent(go.transform, true);
    }

    public void Detach()
    {
        transform.SetParent(null, true);
    }

    public void TeleportToClosest(Transform[] transforms)
    {
        var t = transforms.GetClosest(transform.position);
        transform.SetPositionAndRotation(t.position, t.rotation);
    }
}
