using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CarouselSelector : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] List<string> _items = new();
    [field: SerializeField] public int Value { get; private set; }
    [SerializeField] UnityEvent<int> _onChange;
    private void OnEnable()
    {
        SetValue(Value);
    }
    private void OnValidate()
    {
        SetValue(Value);
    }
    public void SetValue(int i)
    {
        Value = Mathf.Clamp(i, 0, _items.Count - 1);
        _label.text = _items[Value];
    }
    public void Next()
    {
        Value++;
        if (Value >= _items.Count) Value = 0;
        _label.text = _items[Value];
        _onChange.Invoke(Value);
    }
    public void Previous()
    {
        Value--;
        if (Value < 0) Value = _items.Count - 1;
        _label.text = _items[Value];
        _onChange.Invoke(Value);
    }
}
