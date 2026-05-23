using TMPro;
using UnityEngine;

public class ClothingSelectionLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public void SetClothing(Clothing clothing)
    {
        if (clothing == null) { _text.text = "None"; return; }
        _text.text = clothing.Name;
    }
}
