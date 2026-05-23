using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    private static DeathUI instance;
    private void Awake()
    {
        instance = this;
        _image.gameObject.SetActive(false);
    }
    public static void SetSprite(Sprite sprite)
    {
        instance._image.sprite = sprite;
        instance._image.gameObject.SetActive(true);
    }
}
