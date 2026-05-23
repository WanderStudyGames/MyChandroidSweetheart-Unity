using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDialogueComponents", menuName = "ScriptableObjects/Dialogue/Dialogue Components")]
public class DialogueComponents : ScriptableObject, IDialogueComponents
{
    public Camera Camera { get; set; }
    public Transform FocusObject { get; set; }
}
