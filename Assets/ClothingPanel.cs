using System.Collections.Generic;
using UnityEngine;

public class ClothingPanel : MonoBehaviour
{
    [SerializeField] private ClothingAdder _template;
    [SerializeField] private Clothing[] _clothingList;//
    [SerializeField] private Outfit _outfit;
    private List<ClothingAdder> _adders = new();
    private void OnEnable()
    {
        _outfit.Clothings.OnDataModified += UpdateDisplay;
    }
    private void OnDisable()
    {
        _outfit.Clothings.OnDataModified -= UpdateDisplay;
    }
    private void UpdateDisplay()
    {
        foreach (var a in _adders) { if (a != null) Destroy(a.gameObject); }
        _adders.Clear();
        foreach (Clothing clothing in _clothingList)
        {
            if (_outfit.Clothings.Has(clothing)) continue;
            ClothingAdder adder = Instantiate(_template, transform);
            adder.Clothing = clothing;
            adder.gameObject.SetActive(true);
            _adders.Add(adder);
        }
    }
    private void Start()
    {
        UpdateDisplay();
    }
}
