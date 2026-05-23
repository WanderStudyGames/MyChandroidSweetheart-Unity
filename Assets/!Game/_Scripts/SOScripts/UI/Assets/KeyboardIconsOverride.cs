using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardIconsOverride", menuName = "ScriptableObjects/UI/KeyboardIconsOverride")]
public class KeyboardIconsOverride : KeyboardIcons
{
    [field: SerializeField] public string ControlScheme { get; private set; }
}