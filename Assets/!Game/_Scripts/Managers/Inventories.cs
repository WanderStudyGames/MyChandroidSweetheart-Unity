using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventories", menuName = "ScriptableObjects/Inventory/Inventories")]
public class Inventories : ScriptableObject
{
    public static Inventories Instance { get; private set; }
    [SerializeField] private List<Inventory> _inventories = new();
    public Inventory[] List => _inventories.ToArray();
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private Inventory _companionInventory;
    public Inventory PlayerInventory => _playerInventory;
    public Inventory CompanionInventory => _companionInventory;
    private void OnEnable()
    {
        Instance = this;
        PlayerAbilityManager.Inventory = _playerInventory;
        CompanionAbilityManager.Inventory = _companionInventory;
        Init();
    }

    private void Init()
    {

    }
}