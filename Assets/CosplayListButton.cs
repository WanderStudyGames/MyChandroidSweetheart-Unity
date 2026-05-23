using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosplayListButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Button _button;
    [SerializeField] private CosplayCrafter _crafter;
    private Clothing _clothing;
    public void SelectClothing()
    {
        _crafter.Select(_clothing);
    }
    public void SetClothing(Clothing clothing)
    {
        _clothing = clothing;
        _title.text = clothing.Name.ToUpper();
    }
    public void SetColor(Color color)
    {
        ColorBlock c = _button.colors;
        c.normalColor = color;
        _button.colors = c;
    }
    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable;
    }
}
