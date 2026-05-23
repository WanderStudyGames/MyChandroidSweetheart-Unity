using UnityEngine;

[CreateAssetMenu(fileName = "GamepadIcons", menuName = "ScriptableObjects/UI/GamepadIcons")]
public class GamepadIcons : ScriptableObject
{
    [field: SerializeField] public Sprite South { get; private set; }
    [field: SerializeField] public Sprite North { get; private set; }
    [field: SerializeField] public Sprite West { get; private set; }
    [field: SerializeField] public Sprite East { get; private set; }
    [field: SerializeField] public Sprite Start { get; private set; }
    [field: SerializeField] public Sprite Select { get; private set; }
    [field: SerializeField] public Sprite LeftStick { get; private set; }
    [field: SerializeField] public Sprite LSClick { get; private set; }
    [field: SerializeField] public Sprite RightStick { get; private set; }
    [field: SerializeField] public Sprite RSClick { get; private set; }
    [field: SerializeField] public Sprite DPadY { get; private set; }
    [field: SerializeField] public Sprite DPadX { get; private set; }
    [field: SerializeField] public Sprite DPadU { get; private set; }
    [field: SerializeField] public Sprite DPadD { get; private set; }
    [field: SerializeField] public Sprite DPadL { get; private set; }
    [field: SerializeField] public Sprite DPadR { get; private set; }
    [field: SerializeField] public Sprite RB { get; private set; }
    [field: SerializeField] public Sprite RT { get; private set; }
    [field: SerializeField] public Sprite LB { get; private set; }
    [field: SerializeField] public Sprite LT { get; private set; }

}