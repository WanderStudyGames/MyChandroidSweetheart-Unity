using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothingAdder : MonoBehaviour
{
    public Clothing Clothing;
    [SerializeField] private Outfit _outfit;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private Color _enabledTextColor;
    [SerializeField] private Color _disabledTextColor;
    private void Start()
    {
        UpdateDisplay();
        _outfit.Clothings.OnDataModified += UpdateDisplay;
    }
    private void OnDestroy()
    {
        _outfit.Clothings.OnDataModified -= UpdateDisplay;
    }
    private void UpdateDisplay()
    {
        bool b = (Clothing.Icon == null);
        bool disabled = _outfit.Clothings.Has(Clothing);
        _button.interactable = !disabled;
        if (_label == null) return;
        _label.enabled = b;
        _label.text = Clothing.Name;
        _label.color = disabled ? _disabledTextColor : _enabledTextColor;
        if (_image == null) return;
        _image.enabled = !b;
        _image.sprite = Clothing.Icon;
        _image.color = disabled ? Color.gray : Color.white;
    }
    public void AddClothing()
    {
        _outfit.Clothings.Add(Clothing);
    }
    public void RemoveClothing()
    {
        _outfit.Clothings.Remove(Clothing);
    }
}
