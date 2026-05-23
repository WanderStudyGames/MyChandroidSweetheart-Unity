using System;
using UnityEngine;
//public static class PlayerAbilities
//{
//    public static GameObject GameObjectInstance { get; set; }
//    [RuntimeInitializeOnLoadMethod]
//    private static void Init() { Inventory.OnInventoryChange += OnInventoryChanged; }

//    private static List<PlayerAbilityProfile> _abilitiesCache = new();
//    public static event Action<Type, bool> OnSetAbility;
//    public static void SetAbilityEnabled(PlayerAbilityProfile ability, bool b)
//    {
//        //create item by grabbing metadata object using ability reference
//        //add item to playerinventory
//        //Inventories.Instance.PlayerInventory.AddItem()
//    }
//    public static bool HasAbility(Type componentType)
//    {
//        bool success = false;
//        foreach (PlayerAbilityProfile a in _abilitiesCache)
//        {
//            if (componentType == a.GetComponentType()) success = true;
//        }
//        return success;
//    }
//    private static void Add(PlayerAbilityProfile profile)
//    {
//        _abilitiesCache.AddUnique(profile);
//        if (AbilityUtility.TryAddComponent(profile, GameObjectInstance))
//            OnSetAbility?.Invoke(profile.GetComponentType(), true);
//    }
//    private static void Remove(PlayerAbilityProfile profile)
//    {
//        _abilitiesCache.RemoveAll(profile);
//        if (AbilityUtility.TryRemoveComponent(profile, GameObjectInstance))
//            OnSetAbility?.Invoke(profile.GetComponentType(), false);
//    }
//    private static void OnInventoryChanged(Inventory inventory, InventoryItem item, bool added)
//    {
//        if (Inventories.Instance == null) return;
//        if (inventory != Inventories.Instance.PlayerInventory) return;
//        if (GameObjectInstance == null) return;
//        if (AbilityUtility.TryGetProfileFromMetadata(item.Metadata, out PlayerAbilityProfile profile))
//        {
//            if (added) _abilitiesCache.AddUnique(profile);
//            else _abilitiesCache.RemoveAll(profile);
//            if (AbilityUtility.TrySetComponent(profile, GameObjectInstance, added))
//            {
//                OnSetAbility?.Invoke(profile.GetComponentType(), added);
//            }

//        }
//    }
//    public static void RefreshComponents()
//    {

//        var playerInventoryItems = Inventories.Instance.PlayerInventory.GetItems();

//        List<PlayerAbilityProfile> inventoryAbilityProfiles = new();
//        foreach (InventoryItem item in playerInventoryItems)
//        {
//            if (AbilityUtility.TryGetProfileFromMetadata(item.Metadata, out PlayerAbilityProfile profile))
//            {
//                inventoryAbilityProfiles.AddUnique(profile);
//            }
//        }

//        foreach (var profile in inventoryAbilityProfiles)
//        {
//            Add(profile);
//        }

//        for (int i = _abilitiesCache.Count - 1; i >= 0; i--)
//        {
//            bool hasAbility = inventoryAbilityProfiles.Contains(_abilitiesCache[i]);
//            if (hasAbility) continue;

//            Remove(_abilitiesCache[i]);
//        }

//        foreach (InventoryItem item in playerInventoryItems)
//        {
//            if (AbilityUtility.TryGetProfileFromMetadata(item.Metadata, out PlayerAbilityProfile profile))
//            {
//                Add(profile);
//            }
//        }
//    }
//}
public static class AbilityUtility
{
    public static bool TryGetProfileFromMetadata<T>(InventoryItemMetadata itemMetadata, out T componentProfile) where T : ComponentProfile
    {
        var componentItemMetadata = itemMetadata as InventoryComponentItemMetadata;
        if (componentItemMetadata == null) { componentProfile = null; return false; }

        componentProfile = componentItemMetadata.Profile as T;
        return componentProfile != null;
    }
    public static bool TryAddComponent<T>(T profile, GameObject playerObject) where T : ComponentProfile
    {
        Type componentType = profile.GetComponentType();
        if (componentType == null) return false;

        if (playerObject.GetComponent(componentType) == null)
        {
            ObjectComponent pc = (ObjectComponent)playerObject.AddComponent(componentType);
            if (profile != null) { pc.SetComponentProfile(profile); }
            return true;
        }
        return false;
    }
    public static bool TryRemoveComponent<T>(T profile, GameObject playerObject) where T : ComponentProfile
    {
        Type componentType = profile.GetComponentType();
        if (componentType == null) return false;

        if (playerObject.TryGetComponent(componentType, out Component cpn))
        {
            UnityEngine.Object.Destroy(cpn);
            return true;
        }

        return false;
    }
    public static bool TrySetComponent<T>(T profile, GameObject playerObject, bool enabled) where T : ComponentProfile
    {
        if (!enabled)
        {
            return TryRemoveComponent(profile, playerObject);
        }
        else
        {
            return TryAddComponent(profile, playerObject);
        }
    }
}

