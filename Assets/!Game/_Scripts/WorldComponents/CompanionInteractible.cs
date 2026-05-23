using UnityEngine;
using UnityEngine.Events;

public class CompanionInteractible : MonoBehaviour
{
    [SerializeField] private bool interactOnce = false;
    [SerializeField] private UnityEvent OnInteract;
    [SerializeField] private UnityEvent<Companion> OnInteractCompanion;
    [SerializeField] private SFXSource _sfxSource;
    [SerializeField] private Inventory _requiredItemInventory;
    [SerializeField] private InventoryItemMetadata _requiredItem;
    [SerializeField] private UnityEvent OnInteractEnable;
    [SerializeField] private UnityEvent OnInteractDisable;
    [field: SerializeField] public bool UseArms { get; private set; } = false;
    [field: SerializeField] private bool _canInteract = true;
    public bool CanInteract
    {
        get => _canInteract;
        set
        {
            _canInteract = value;
            if (value) OnInteractEnable.Invoke();
            else OnInteractDisable.Invoke();
        }
    }
    private bool _hasRequiredItem = false;
    public bool IsInteractable()
    {
        return CanInteract && _hasRequiredItem;
    }
    private void OnEnable()
    {
        Inventory.OnInventoryChange += OnInventoryChange;
    }
    private void OnDisable()
    {
        Inventory.OnInventoryChange -= OnInventoryChange;
    }
    private void OnInventoryChange(Inventory inventory, InventoryItem item, bool added)
    {
        if (inventory == _requiredItemInventory && item?.Metadata == _requiredItem)
        {
            _hasRequiredItem = added;
        }
    }
    private void Start()
    {
        _hasRequiredItem = _requiredItemInventory == null || _requiredItem == null || _requiredItemInventory.Has(_requiredItem);
    }
    [field: SerializeField] public GameObject AlternativePositionCheck;
    public void InteractAction(Companion companion)
    {
        if (companion.CarriedObject != null) return;
        if (_requiredItemInventory != null && _requiredItem != null && !_requiredItemInventory.Has(_requiredItem)) return;
        if (CanInteract)
            OnInteract.Invoke();
        OnInteractCompanion.Invoke(companion);
        if (interactOnce)
            CanInteract = false;
        if (_sfxSource != null)
            _sfxSource.Play();
    }
}
