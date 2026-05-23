using UnityEngine;
using UnityEngine.Events;

public class InventoryListener : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _inventoryItemMetadata;
    [SerializeField] private UnityEvent _execute;
    private void Awake()
    {
        Inventory.OnInventoryChange += OnInventoryChange;
    }
    private void OnDestroy()
    {
        Inventory.OnInventoryChange -= OnInventoryChange;
    }
    private void OnInventoryChange(Inventory inventory, InventoryItem inventoryItem, bool b)
    {
        if (b && inventory == _inventory && inventoryItem.Metadata == _inventoryItemMetadata)
        {
            _execute.Invoke();
        }
    }
}
