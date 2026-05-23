using TMPro;
using UnityEngine;

public class ClothingRemover : MonoBehaviour
{

    [SerializeField] private ClothingEditor _clothingEditor;
    public Outfit Outfit;
    private Clothing _clothing;
    public Clothing Clothing
    {
        get { return _clothing; }
        set
        {
            _clothing = value;
            UpdateDisplay();
        }
    }
    [SerializeField] private TMP_Text _label;
    private void UpdateDisplay()
    {
        _label.text = Clothing.Name;
    }
    public void RemoveClothing()
    {
        Outfit.Clothings.Remove(Clothing);
    }
    public void SelectClothing()
    {
        _clothingEditor.Select(_clothing);
    }
}
