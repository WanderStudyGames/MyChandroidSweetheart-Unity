using UnityEngine;
[CreateAssetMenu(fileName = "Character Dialogue Profile", menuName = "ScriptableObjects/Dialogue/Character Profile")]
public class CharacterProfile : ScriptableObject, ICharacterProfile
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Color NameColor { get; private set; } = Color.white;
    [SerializeField] private Color color = Color.white;
    public Color Color => color;
}