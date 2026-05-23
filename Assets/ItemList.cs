using TMPro;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] private ItemDisplay _template;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _description;
    // Start is called before the first frame update

    void Start()
    {
        UpdateList();
    }
    private void Awake()
    {
        Inventory.OnInventoryChange += OnInventoryChange;

    }
    private void OnDestroy()
    {
        Inventory.OnInventoryChange -= OnInventoryChange;
    }
    private void OnInventoryChange(Inventory inv, InventoryItem item, bool b)
    {
        if (inv == _inventory)
        {
            UpdateList();
        }
    }
    private void UpdateList()
    {
        _description.text = "";
        foreach (Transform tr in transform.GetComponentInChildren<Transform>())
        {
            Destroy(tr.gameObject);
        }
        foreach (var item in _inventory.GetItems())
        {
            var id = Instantiate(_template, transform);
            id._itemMetadata = item.Metadata;
            id._description = _description;
        }
    }
}
