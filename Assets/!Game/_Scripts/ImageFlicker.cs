using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageFlicker : MonoBehaviour
{
    [SerializeField] private Color _secondaryColor = Color.white;
    [SerializeField, Range(1, 60)] private int _fps = 1;
    private Image _image;
    private Color _primaryColor;
    private void Toggle()
    {
        if (_image.color != _secondaryColor) { _image.color = _secondaryColor; }
        else { _image.color = _primaryColor; }
    }
    private void Awake()
    {
        _image = GetComponent<Image>();
        _primaryColor = _image.color;
    }
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Flicker());
    }

    private IEnumerator Co_Flicker()
    {
        if (_fps <= 0) yield break;
        var wait = new WaitForSecondsRT(1 / _fps);
        while (true)
        {
            Toggle();
            yield return wait.NewTime(1 / _fps);
        }
    }
}
