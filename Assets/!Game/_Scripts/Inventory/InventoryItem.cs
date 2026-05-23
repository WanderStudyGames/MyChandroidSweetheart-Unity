using System;
using UnityEngine;
[Serializable]
public class InventoryItem
{
    [field: SerializeField] public InventoryItemMetadata Metadata { get; private set; }
    public InventoryItem(InventoryItemMetadata metadata)
    {
        Metadata = metadata;
    }
}