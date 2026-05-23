using System;
using UnityEngine;
using UnityEngine.Events;

public class SprocketInteractible : MonoBehaviour, ISprocketPushable
{

    [SerializeField] private UnityEvent execute;

    [SerializeField] private bool interactOnce;

    [SerializeField][Range(0, 1)] private float chargeThreshhold = 1f;

    [SerializeField] private bool _canLaunchPlayer;

    [SerializeField] private InventoryItemMetadata _requiredItem;
    [SerializeField] private Inventory _inventory;

    public float ChargeThreshhold => chargeThreshhold;

    public bool canInteract { get; set; } = true;
    private bool hasRequiredItem = false;

    private void Start()
    {
        if (_requiredItem == null || _inventory == null)
        {
            hasRequiredItem = true;
        }
        else
        {
            hasRequiredItem = _inventory.Has(_requiredItem);
        }
    }
    private void OnEnable()
    {
        Inventory.OnInventoryChange += OnInventoryChange;
    }

    private void OnInventoryChange(Inventory inventory, InventoryItem item, bool enabled)
    {
        if (inventory != _inventory || item.Metadata != _requiredItem) return;
        hasRequiredItem = enabled;
    }

    public bool Push(Vector3 force)
    {
        bool b = _canLaunchPlayer;
        var chargeAmount = force.magnitude;
        if (!canInteract || !hasRequiredItem)
        {
            Debug.Log("SprocketInteractible cannot interact. canInteract: " + canInteract + ", hasRequiredItem: " + hasRequiredItem);
            return b;
        }
        if (execute.GetPersistentEventCount() < 1) return b;
        if (chargeAmount < chargeThreshhold) return b;

        Debug.Log("SprocketInteractible invoked by sprocket with charge amount: " + chargeAmount);
        execute.Invoke();
        Debug.Log("SprocketInteractible invoked execute UnityEvent.");
        if (interactOnce) canInteract = false;
        Debug.Log("SprocketInteractible interaction complete.");
        return b;
    }
}
