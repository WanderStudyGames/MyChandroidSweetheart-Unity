using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCost : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amount;

    public void SetDisplay(Sprite icon, Color color, int amount)
    {
        _icon.sprite = icon;
        _icon.color = color;
        _amount.text = amount.ToString("N0");
    }
}
