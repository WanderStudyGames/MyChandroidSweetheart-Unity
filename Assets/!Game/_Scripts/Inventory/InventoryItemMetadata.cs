using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/Inventory Item Metadata", fileName = "Inventory Item Metadata")]
public class InventoryItemMetadata : ScriptableObject
{
    [field: SerializeField, FormerlySerializedAs("_name")] public string Name { get; private set; }
    [field: SerializeField, TextArea, FormerlySerializedAs("_description")] public string Description { get; private set; }
    [field: SerializeField, FormerlySerializedAs("_icon")] public Sprite Icon { get; private set; }
}
