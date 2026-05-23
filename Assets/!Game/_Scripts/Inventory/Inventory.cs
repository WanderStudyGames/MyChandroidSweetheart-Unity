using QFSW.QC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/Inventory", fileName = "Inventory")]
public class Inventory : ScriptableObject
{
    public static event Action<Inventory, InventoryItem, bool> OnInventoryChange;

    [SerializeField] private GameEvent _onInventoryChange;
    [BeginReadOnlyGroup, SerializeField] private List<InventoryItem> _items = new();
    [EndReadOnlyGroup]
    [SerializeField] private InventoryItem[] _defaultItems = { };
#if UNITY_EDITOR
    public bool LoadDebugInventory;
    [SerializeField] private InventoryItem[] _editorDefaultItems = { };
#endif
    private void OnEnable()
    {
        SaveSystem.OnLoadFile += Load;
        SaveSystem.OnSaveFile += Save;
        Load(SaveSystem.Files);
    }
    private void OnDisable()
    {
        SaveSystem.OnLoadFile -= Load;
        SaveSystem.OnSaveFile -= Save;
    }
    private void Save(SaveSystem.SaveFileNames files)
    {
        string path;
        if (name == "Player Inventory") { path = files.playerData; }
        else if (name == "Companion Inventory") { path = files.companionData; }
        else { return; }
#if UNITY_EDITOR
        if (LoadDebugInventory) return;
#endif
        var strings = new List<string>();
        foreach (var item in _items)
        {
            if (item != null && item.Metadata != null) strings.Add(item.Metadata.name);
        }
        ES3.Save("inventory", strings, path);
    }
    private void Load(SaveSystem.SaveFileNames files)
    {
        var path = "";
        if (name == "Player Inventory") { path = files.playerData; }
        else if (name == "Companion Inventory") { path = files.companionData; }
        else { return; }

#if UNITY_EDITOR
        if (LoadDebugInventory) { _items = _editorDefaultItems.ToList(); return; }
#endif

        var strings = ES3.Load("inventory", path, new List<string>());
        if (strings.Count > 0)
        {
            _items.Clear();
            foreach (var str in strings)
            {
                var itemMetadata = Resources.Load<InventoryItemMetadata>(str);
                if (itemMetadata == null) continue;
                if (itemMetadata is InventoryComponentItemMetadata) itemMetadata = (InventoryComponentItemMetadata)itemMetadata;
                _items.Add(new(itemMetadata));
            }
        }
        else
        {
            _items = _defaultItems.ToList();
        }
    }
    public bool Has(InventoryItemMetadata inventoryItemMetadata)
    {
        return _items.Any(item => item.Metadata == inventoryItemMetadata);
    }
    public bool Has(string name)
    {
        return _items.Any(item => item.Metadata.Name == name);
    }
    [HideInInspector] public InventoryItem[] GetItems() => _items.ToArray();



    public void AddItem(InventoryItem item)
    {
        if (Has(item.Metadata)) return;
        _items.AddUnique(item);
        OnInventoryChange?.Invoke(this, item, true);
        _onInventoryChange?.Raise();
    }
    public void AddItem(InventoryItemMetadata metadata)
    {
        InventoryItem item = new(metadata);
        AddItem(item);
        SaveNotification.Show();
    }
    private void RemoveItem(InventoryItemMetadata item)
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            if (_items[i].Metadata == item)
            {
                var it = _items[i];
                _items.Remove(_items[i]);
                OnInventoryChange?.Invoke(this, it, false);
                _onInventoryChange?.Raise();
            }
        }
        SaveNotification.Show();
    }
    public void SetItemAdded(InventoryItemMetadata item, bool b)
    {
        if (b) AddItem(item);
        else RemoveItem(item);
    }
    private void Clear()
    {
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            var it = _items[i];
            _items.Remove(_items[i]);
            OnInventoryChange?.Invoke(this, it, false);
        }
        SaveNotification.Show();
    }
    public void TransferItem(int index, Inventory newInventory)
    {
        if (index > -1 && index < _items.Count)
        {
            TransferItem(_items[index], newInventory);
        }
    }
    public void TransferItem(InventoryItem item, Inventory newInventory)
    {
        if (newInventory == this) return;
        newInventory.AddItem(item);
        RemoveItem(item.Metadata);
        OnInventoryChange?.Invoke(this, item, false);
    }

    public void TransferItem(InventoryItemMetadata itemMetadata, Inventory newInventory)
    {
        var item = _items.First(it => it.Metadata == itemMetadata);
        if (item != null) { TransferItem(item, newInventory); }
    }
    public void TransferLastItem(Inventory newInventory)
    {
        if (_items.Count < 1) return;
        TransferItem(_items[^1], newInventory);
    }
    enum Items
    {
        MetalBoots,
        QuanTravDevice
    }
    [Command("inventory-give")]
    private static string EquipmentAdd([InventoryTag] string inventory, [InventoryItemMetadataTag] string item)

    {
        Resources.Load<Inventory>(inventory).AddItem(Resources.Load<InventoryItemMetadata>(item));
        return $"Added {item} to {inventory}.";
    }
    [Command("inventory-list")]
    private static IEnumerable<string> EquipmentList([InventoryTag] string inventory)
    {
        var items = Resources.Load<Inventory>(inventory).GetItems();
        foreach (var item in items)
        {
            yield return item.Metadata.Name;
        }
    }
    [Command("inventory-remove")]
    private static string EquipmentRemove([InventoryTag] string inventory, [InventoryItemMetadataTag] string item)
    {
        Resources.Load<Inventory>(inventory).RemoveItem(Resources.Load<InventoryItemMetadata>(item));
        return $"Removed all {item} from {inventory}.";
    }
    [Command("inventory-clear")]
    private static string EquipmentClear([InventoryTag] string inventory)
    {
        Resources.Load<Inventory>(inventory).Clear();
        return $"Removed all items from {inventory}";
    }
}
public struct InventoryTag : IQcSuggestorTag { }
public sealed class InventoryTagAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new InventoryTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}
public class InventorySuggestor : BasicCachedQcSuggestor<Inventory>
{
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<InventoryTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(Inventory inventory)
    {
        return new RawSuggestion(inventory.name, true);
    }

    protected override IEnumerable<Inventory> GetItems(SuggestionContext context, SuggestorOptions options)
    {
        return Resources.LoadAll<Inventory>("");
    }
}
public struct InventoryItemMetadataTag : IQcSuggestorTag { }
public sealed class InventoryItemMetadataTagAttribute : SuggestorTagAttribute
{
    private readonly IQcSuggestorTag[] _tags = { new InventoryItemMetadataTag() };

    public override IQcSuggestorTag[] GetSuggestorTags()
    {
        return _tags;
    }
}
public class InventoryItemMetadataSuggestor : BasicCachedQcSuggestor<InventoryItemMetadata>
{
    public static Inventory Inventory;
    protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
    {
        return context.HasTag<InventoryItemMetadataTag>();
    }

    protected override IQcSuggestion ItemToSuggestion(InventoryItemMetadata item)
    {
        return new RawSuggestion(item.name, true);
    }

    protected override IEnumerable<InventoryItemMetadata> GetItems(SuggestionContext context, SuggestorOptions options)
    {

        foreach (var tag in context.Tags) { Debug.Log(tag.GetType()); }
        Debug.Log(context.Depth);
        Debug.Log(context.Prompt);
        Debug.Log(context.TargetType);

        return Resources.LoadAll<InventoryItemMetadata>("");
    }
}
