using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VInspector;

public class Fork : MonoBehaviour
{
    public enum Conditions
    {
        WorldFlag,
        SceneBool,
        InventoryItem,
        LocalBoolean,
        StaticFlag,
        DiscoveredRoom,
        Spawnpoint,
        KioskMode,
        SavesActivated
    }
    public Conditions Condition;
    [SerializeField, ShowIf("Condition", Conditions.WorldFlag)] private string _worldFlag;
    [SerializeField, ShowIf("Condition", Conditions.SceneBool)] private SceneAssetReference _sceneBool;
    [SerializeField, ShowIf("Condition", Conditions.InventoryItem)] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _inventoryItem;
    [SerializeField, ShowIf("Condition", Conditions.StaticFlag)] private string _staticFlag;
    [SerializeField, ShowIf("Condition", Conditions.DiscoveredRoom)] private bool _useCurrentScene;
    [SerializeField, ShowIf("Condition", Conditions.DiscoveredRoom)] private SceneAssetReference _discoveredRoom;
    [SerializeField, ShowIf("Condition", Conditions.Spawnpoint)] private int _spawnpoint;
    [EndIf]
    [SerializeField] private UnityEvent _true;
    [SerializeField] private UnityEvent _false;
    [SerializeField] private bool _executeOnAwake;
    private bool _localBool;
    private void Awake()
    {
        if (_executeOnAwake) { Execute(); }
    }
    public void Execute()
    {
        switch (Condition)
        {
            case Conditions.WorldFlag:
                if (WorldData.WorldFlags.Has(_worldFlag)) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.SceneBool:
                if (WorldData.SceneBools.Has(_sceneBool.SceneName)) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.InventoryItem:
                if (_inventory == null || _inventoryItem == null) break;
                if (_inventory.Has(_inventoryItem)) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.LocalBoolean:
                if (_localBool) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.StaticFlag:
                if (WorldData.StaticFlags.Has(_staticFlag)) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.DiscoveredRoom:
                var sceneName = _useCurrentScene ? SceneManager.GetActiveScene().name : _discoveredRoom?.SceneName;
                if (WorldData.DiscoveredRooms.Has(sceneName)) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.Spawnpoint:
                if (SceneHandler.DestSpawnpoint == _spawnpoint) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.KioskMode:
                if (SaveSystem.IsKiosk) _true.Invoke();
                else _false.Invoke();
                break;
            case Conditions.SavesActivated:
                if (SaveSystem.CanSave) _true.Invoke();
                else _false.Invoke();
                break;
        }
    }
    public void SaveTrue()
    {
        SetBool(true);
    }
    public void SetBool(bool b)
    {
        switch (Condition)
        {
            case Conditions.WorldFlag:
                WorldData.WorldFlags.Set(_worldFlag, b);
                break;
            case Conditions.SceneBool:
                WorldData.SceneBools.Set(_sceneBool?.SceneName, b);
                break;
            case Conditions.InventoryItem:
                if (_inventory != null) _inventory.SetItemAdded(_inventoryItem, b);
                break;
            case Conditions.LocalBoolean:
                _localBool = b;
                break;
            case Conditions.StaticFlag:
                WorldData.StaticFlags.Set(_staticFlag, b);
                Debug.LogError($"SET{_staticFlag} {b}!!!!!!!");
                break;
        }
    }
}
