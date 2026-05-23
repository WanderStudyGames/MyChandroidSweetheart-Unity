using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _metadata;
    public void Give()
    {
        if (_inventory.Has(_metadata)) return;
        _inventory.AddItem(new InventoryItem(_metadata));
    }
}
