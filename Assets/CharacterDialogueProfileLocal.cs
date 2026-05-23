using UnityEngine;

public class CharacterDialogueProfileLocal : MonoBehaviour, ICharacterProfile
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public Color NameColor { get; private set; } = Color.white;
}
