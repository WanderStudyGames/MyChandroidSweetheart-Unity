using System;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardIcons", menuName = "ScriptableObjects/UI/KeyboardIcons")]
public class KeyboardIcons : ScriptableObject
{

    [SerializeField] private BindingImage[] _bindingImages;
    [SerializeField] private KeyboardIconsOverride[] _keyboardIconsOverrides;
    [SerializeField] private JoystickControlPathOverrides _joystickControlPathOverrides;
    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Init;
        SaveSystem.OnSaveStatic += Save;
        Init();
    }
    private static void Save()
    {
        ES3.Save("controllerIconType", _iconType, "settings.es3");
    }
    private static void Init()
    {
        _iconType = ES3.Load("controllerIconType", "settings.es3", IconTypes.Auto);
    }
    public static event Action OnIconTypeChanged;
    public static void SetIconType(int type)
    {
        type = Mathf.Clamp(type, 0, 3);
        _iconType = (IconTypes)type;
        OnIconTypeChanged?.Invoke();
    }
    public enum IconTypes
    {
        Auto,
        Chief,
        Bandicoot,
        Plumber
    }
    private static IconTypes _iconType = IconTypes.Auto;
    public static IconTypes IconType => _iconType;
    public bool TryGetSprite(string input, string controlScheme, out BindingImage bindingImage)
    {
        input = _joystickControlPathOverrides.GetOverride(input);
        switch (_iconType)
        {
            case IconTypes.Chief:
                controlScheme = "Xbox";
                break;
            case IconTypes.Bandicoot:
                controlScheme = "PlayStation";
                break;
            case IconTypes.Plumber:
                controlScheme = "Switch";
                break;
            default: break;
        }
        foreach (var over in _keyboardIconsOverrides)
        {
            if (controlScheme.Contains(over.ControlScheme) && over.TryGetSprite(input, controlScheme, out bindingImage))
            {
                return true;
            }
        }

        bindingImage = default;
        if (string.IsNullOrEmpty(input)) return false;

        foreach (var currentImage in _bindingImages)
        {

            if (input.Contains(currentImage.key)) { bindingImage = currentImage; return true; }

        }

        return false;
    }
}
[Serializable]
public struct BindingImage
{
    public string key;
    public Sprite sprite;
    public bool ignoreTint;
}
