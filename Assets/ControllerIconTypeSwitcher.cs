using UnityEngine;
using UnityEngine.UI;

public class ControllerIconTypeSwitcher : MonoBehaviour
{
    [SerializeField] private KeyboardIcons _keyboardIcons;
    [SerializeField] private CarouselSelector _carouselSelector;
    [SerializeField, Header("Images")] private Image _buttonEast;
    [SerializeField] private Image _buttonSouth;
    [SerializeField] private Image _buttonNorth;
    [SerializeField] private Image _buttonWest;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _carouselSelector.SetValue((int)KeyboardIcons.IconType);
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        UpdateButton("buttonEast", _buttonEast);
        UpdateButton("buttonSouth", _buttonSouth);
        UpdateButton("buttonNorth", _buttonNorth);
        UpdateButton("buttonWest", _buttonWest);
    }
    public void SetType(int i)
    {
        KeyboardIcons.SetIconType(i);
        UpdateDisplay();
    }
    // Update is called once per frame
    void UpdateButton(string button, Image image)
    {
        if (_keyboardIcons.TryGetSprite(button, GlobalPlayerInput.ControlScheme, out BindingImage bindingImage))
        {
            image.sprite = bindingImage.sprite;
        }
    }
}
