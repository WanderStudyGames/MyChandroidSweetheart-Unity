using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/Inventory Component Item Metadata", fileName = "Inventory Component Item Metadata")]
public class InventoryComponentItemMetadata : InventoryItemMetadata
{
    [SerializeField] private ComponentProfile _profile;
    public ComponentProfile Profile => _profile;
}