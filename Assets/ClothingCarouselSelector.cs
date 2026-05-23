using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClothingCarouselSelector : MonoBehaviour
{
    [SerializeField] private Clothing.ClothingType _clothingType;
    [SerializeField] private Wardrobe _wardrobe;
    [SerializeField] private Outfit _outfit;
    [SerializeField] private TMP_Text _tallyText;

    [SerializeField] private UnityEvent<Clothing> _onRefreshDisplay;

    private int _selectionIndex;
    private List<Clothing> _clothings;

    public Clothing Clothing => _clothings[_selectionIndex];

    private void Awake()
    {
        GetClothings();
        _selectionIndex = FindSelectionIndex();
        RefreshDisplay();
    }
    public void SetClothingType(int type)
    {
        _clothingType = (Clothing.ClothingType)type;
        Awake();
    }
    private void GetClothings()
    {
        _clothings = _wardrobe.Clothings.GetValues().Where((cl) => { return cl.Type == _clothingType; }).ToList();
        _clothings = _clothings.Prepend(null).ToList();
    }
    private int FindSelectionIndex()
    {
        Clothing selection = null;
        foreach (Clothing clothing in _outfit.Clothings.GetValues())
        {
            if (_clothingType == clothing.Type) { selection = clothing; break; }
        }
        for (int i = 0; i < _clothings.Count; i++)
        {
            if (selection == _clothings[i]) return i;
        }
        if (selection == null) { return 0; }
        _clothings.AddUnique(selection);
        return _clothings.Count - 1;
    }
    public void Next()
    {
        _selectionIndex++;
        if (_selectionIndex > _clothings.Count - 1) { _selectionIndex = 0; }
        RefreshDisplay();
    }
    public void Previous()
    {
        _selectionIndex--;
        if (_selectionIndex < 0) { _selectionIndex = _clothings.Count - 1; }
        RefreshDisplay();
    }
    private void RefreshDisplay()
    {
        _tallyText.text = $"{_selectionIndex}/{_clothings.Count - 1}";
        _outfit.RemoveAllClothingOfType(_clothingType);
        if (Clothing != null) _outfit.Clothings.Add(Clothing);
        _onRefreshDisplay.Invoke(Clothing);
    }
}
