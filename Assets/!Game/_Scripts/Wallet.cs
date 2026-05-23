using System;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/Wallet", fileName = "Wallet")]
public class Wallet : ScriptableObject
{
    [field: SerializeField] public Sprite Icon;
    [field: SerializeField] public Color Color;
    private void OnEnable()
    {
        SaveSystem.OnSaveFile += Save;
        SaveSystem.OnLoadFile += Load;
        Load(SaveSystem.Files);
    }
    private void OnDisable()
    {
        SaveSystem.OnSaveFile -= Save;
        SaveSystem.OnLoadFile -= Load;
    }
    private void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save(name, Currency, files.playerData);
    }
    private void Load(SaveSystem.SaveFileNames files)
    {
        Currency = ES3.Load(name, files.playerData, 0);
    }
    public int Currency { get; private set; }
    public event Action<int> OnCurrencyChanged;
    public void Add(int amount)
    {
        Currency += amount;
        OnCurrencyChanged?.Invoke(amount);
    }
    public void Set(int currency)
    {
        var old = Currency;
        Currency = currency;
        OnCurrencyChanged?.Invoke(currency - old);
    }
    public void Clear()
    {
        var old = Currency;
        Currency = 0; OnCurrencyChanged?.Invoke(-Currency);
    }
}