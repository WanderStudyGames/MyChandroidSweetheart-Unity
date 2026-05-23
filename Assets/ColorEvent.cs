using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorEvent : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private Image _image;
    [SerializeField] private UnityEvent<Color> _event;
    private void OnValidate()
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        if (_image != null)
            _image.color = _color;
    }
    public void SetColor(Color color) { _color = color; UpdateDisplay(); }
    public void InvokeColorEvent()
    {
        _event.Invoke(_color);
    }

}
