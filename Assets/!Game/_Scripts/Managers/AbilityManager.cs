using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class AbilityManager<TComponentProfile> where TComponentProfile : ComponentProfile
{
    public static GameObject GameObjectInstance { get; set; }
    public static event Action<Type, bool> OnSetAbility;
    public static Inventory Inventory { private get; set; }

    private static List<TComponentProfile> _profileCache = new();
    public static void GiveAbilityItem(TComponentProfile ability)
    {
        Resources.LoadAll<InventoryItemMetadata>("");
        var items = Resources.FindObjectsOfTypeAll<InventoryItemMetadata>();
        foreach (var item in items)
        {

            if (AbilityUtility.TryGetProfileFromMetadata(item, out TComponentProfile pap))
            {
                if (pap == ability) { Inventory.AddItem(item); }
            }
        }
    }
    public static bool HasAbility(Type componentType)
    {
        bool success = false;
        foreach (TComponentProfile a in _profileCache)
        {
            if (componentType == a.GetComponentType()) success = true;
        }
        return success;
    }
    private static void Add(TComponentProfile profile)
    {
        _profileCache.AddUnique(profile);
        if (AbilityUtility.TryAddComponent(profile, GameObjectInstance))
            OnSetAbility?.Invoke(profile.GetComponentType(), true);
    }
    private static void Remove(TComponentProfile profile)
    {
        _profileCache.RemoveAll(profile);
        if (AbilityUtility.TryRemoveComponent(profile, GameObjectInstance))
            OnSetAbility?.Invoke(profile.GetComponentType(), false);
    }
    protected static void OnInventoryChanged(Inventory inventory, InventoryItem item, bool added)
    {
        if (Inventories.Instance == null) return;
        if (inventory != Inventory) return;
        if (GameObjectInstance == null) return;
        RefreshComponents();
    }
    public static void RefreshComponents()
    {

        var inventoryItems = Inventory.GetItems();

        List<TComponentProfile> inventoryAbilityProfiles = new();
        foreach (InventoryItem item in inventoryItems)
        {
            if (AbilityUtility.TryGetProfileFromMetadata(item.Metadata, out TComponentProfile profile))
            {
                inventoryAbilityProfiles.AddUnique(profile);
            }
        }

        foreach (var profile in inventoryAbilityProfiles)
        {
            Add(profile);
        }

        for (int i = _profileCache.Count - 1; i >= 0; i--)
        {
            bool hasAbility = inventoryAbilityProfiles.Contains(_profileCache[i]);
            if (hasAbility) continue;

            Remove(_profileCache[i]);
        }

        foreach (InventoryItem item in inventoryItems)
        {
            if (AbilityUtility.TryGetProfileFromMetadata(item.Metadata, out TComponentProfile profile))
            {
                Add(profile);
            }
        }
    }
}

public class PlayerAbilityManager : AbilityManager<PlayerAbilityProfile>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init() { Inventory.OnInventoryChange += OnInventoryChanged; }
}
public class CompanionAbilityManager : AbilityManager<CompanionAbilityProfile>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init() { Inventory.OnInventoryChange += OnInventoryChanged; }
}

