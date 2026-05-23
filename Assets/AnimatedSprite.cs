using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AnimatedSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _delay;
    private float time;
    private Image _image;
    private int index;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Update()
    {
        time += Time.unscaledDeltaTime;
        if (time > _delay)
        {
            time = 0;
            index = (index + 1) % _sprites.Length;
            _image.sprite = _sprites[index];
        }
    }
}
