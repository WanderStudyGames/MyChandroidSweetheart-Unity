using System;
using TMPro;
using UnityEngine;

public class SetTMPValue : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmp;
    [SerializeField][Range(0, 5)] private int _decimalPlaces;
    public void SetValue(float f)
    {
        decimal d = (decimal)f;
        d = Decimal.Round((decimal)f, _decimalPlaces);
        _tmp.text = d.ToString();
    }
}
