using TMPro;
using UnityEngine;

public class ValueToText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField, Min(0)] private int _decimalPlaces = 2;
    public void Float(float f)
    {
        var multiplier = Mathf.Pow(10, _decimalPlaces);
        var big = f * multiplier;
        _text.text = (Mathf.Round(big) / multiplier).ToString();
    }
}
