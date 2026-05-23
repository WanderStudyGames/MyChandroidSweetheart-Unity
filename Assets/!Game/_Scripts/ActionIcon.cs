using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _tmpro;
    public Sprite sprite { get { return _image.sprite; } set { _image.sprite = value; } }
    public string text { get { return _tmpro.text; } set { _tmpro.text = value; } }
    public Color color { get { return _image.color; } set { _image.color = value; _tmpro.color = value; } }
    public void Set(Sprite sprite, string text, Color color = default)
    {
        this.sprite = sprite;
        this.text = text;
        if (color != default) { this.color = color; }
    }

}
