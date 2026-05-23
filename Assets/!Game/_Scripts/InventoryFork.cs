using UnityEngine;

public class InventoryFork : MonoBehaviour
{
    [SerializeField] private GameObject _true;
    [SerializeField] private GameObject _false;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _itemMetadata;
    private void Awake()
    {
        Check();
    }
    public void Check()
    {
        bool b = _inventory.Has(_itemMetadata);
        if (_true != null) _true.SetActive(b);
        if (_false != null) _false.SetActive(!b);
    }
}
