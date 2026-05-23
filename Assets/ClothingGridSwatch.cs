using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClothingGridSwatch : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private UnityEvent<Clothing> _onEditClothing;

    [SerializeField] private Toggle _toggle;

    [SerializeField] private ClothingGridSelector _selector;

    private int _index;
    private Clothing _clothing;

    public void EditClothing()
    {
        _onEditClothing.Invoke(_clothing);
    }

    public void SetClothing(Clothing clothing, int index, bool on)
    {
        _index = index;
        _clothing = clothing;
        _text.text = clothing.Name;
        _toggle.isOn = on;
    }

    public void SetClothingEnabled(bool enabled)
    {
        if (enabled)
        {
            _selector.Outfit.Clothings.Add(_clothing);
        }
        else
        {
            _selector.Outfit.Clothings.Remove(_clothing);
        }
    }

}
