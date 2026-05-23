using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CosplayButtonList : MonoBehaviour
{
    [SerializeField] private CosplayListButton _template;
    [SerializeField] private Wardrobe _wardrobe;
    [SerializeField] private Clothing.ClothingType _clothingType = Clothing.ClothingType.Top;
    [SerializeField] private Color _ownedColor = Color.green;
    [SerializeField] private Color _unavailableColor = Color.green;

    private List<CosplayListButton> _cosplayListButtons = new();
    private void Awake()
    {
        _template.gameObject.SetActive(false);
    }
    public void SetClothingType(int type)
    {
        _clothingType = (Clothing.ClothingType)type;
        RefreshDisplay();
    }
    private void OnEnable()
    {
        CosplayCrafter.OnCraftItem += RefreshDisplay;
        RefreshDisplay();
    }
    private void OnDisable()
    {
        CosplayCrafter.OnCraftItem -= RefreshDisplay;

    }
    private void RefreshDisplay()
    {
        foreach (var button in _cosplayListButtons) { if (button != null) Destroy(button.gameObject); }
        _cosplayListButtons.Clear();
        foreach (var blueprint in _wardrobe.Blueprints.GetValues().Where(c => c != null && c.Type == _clothingType))
        {
            var b = Instantiate(_template);
            b.SetClothing(blueprint);
            //b.SetInteractable(!_wardrobe.Clothings.Has(blueprint));
            b.transform.SetParent(_template.transform.parent, false);
            _cosplayListButtons.Add(b);

            if (_wardrobe.Clothings.Has(blueprint)) b.SetColor(_ownedColor);
            if (!blueprint.Affordable()) b.SetColor(_unavailableColor);
            b.gameObject.SetActive(true);

        }
    }
}
