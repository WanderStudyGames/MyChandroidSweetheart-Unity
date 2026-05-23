using System.Collections.Generic;
using UnityEngine;

public class ColorButtonsGenerator : MonoBehaviour
{
    [SerializeField] private ColorPallette _pallette;
    [SerializeField] private ColorEvent _colorEventTemplate;
    private List<ColorEvent> _buttons = new();
    private void Awake()
    {
        _colorEventTemplate.gameObject.SetActive(false);
        GenerateButtons();
    }
    private void GenerateButtons()
    {
        foreach (var button in _buttons)
        {
            Destroy(button.gameObject);
        }
        _buttons.Clear();
        foreach (var b in GetComponentsInChildren<ColorEvent>())
        {
            if (b != _colorEventTemplate)
                Destroy(b.gameObject);
        }
        foreach (var c in _pallette.Swatches)
        {
            var ce = Instantiate(_colorEventTemplate, transform);
            ce.gameObject.SetActive(true);
            ce.SetColor(c);
            _buttons.Add(ce);
        }
        Debug.Log(_buttons.Count);
        Debug.Log(_pallette.Swatches.Length);
    }
}
