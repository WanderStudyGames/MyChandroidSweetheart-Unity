using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ClothingGridSelector : MonoBehaviour
{
    [SerializeField] private Clothing.ClothingType _clothingType;
    [SerializeField] private Wardrobe _wardrobe;
    [field: SerializeField] public Outfit Outfit { get; private set; }
    [SerializeField] private ClothingGridSwatch _swatchTemplate;
    [SerializeField] private RectTransform _swatchParent;

    [SerializeField] private UnityEvent<Clothing> _onRefreshDisplay;

    private List<ClothingGridSwatch> _swatches = new();

    private List<Clothing> _clothings = new();

    private void GenerateSwatches()
    {
        foreach (var sw in _swatches) { Destroy(sw.gameObject); }
        _swatches.Clear();
        for (int i = 0; i < _clothings.Count; i++)
        {
            var sw = Instantiate(_swatchTemplate, _swatchParent);
            sw.SetClothing(_clothings[i], i, Outfit.Clothings.Has(_clothings[i]));
            sw.gameObject.SetActive(true);
            _swatches.Add(sw);
        }
    }
    public void SetClothingType(int type)
    {
        _clothingType = (Clothing.ClothingType)type;
        Awake();
    }
    private void GetClothings()
    {
        _clothings = _wardrobe.Clothings.GetValues().Where((cl) => { return cl.Type == _clothingType; }).ToList();
    }
    private void Awake()
    {
        GetClothings();
        GenerateSwatches();
        _swatchTemplate.gameObject.SetActive(false);
    }

}
