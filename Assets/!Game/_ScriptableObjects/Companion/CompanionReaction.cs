using UnityEngine;

[CreateAssetMenu(fileName = "CompanionReaction", menuName = "ScriptableObjects/CompanionReaction")]
public class CompanionReaction : ScriptableObject
{
    [SerializeField] private AnimationClip _reaction;
    public AnimationClip Reaction => _reaction;
    [SerializeField] private AnimationClip _posture;
    public AnimationClip Posture => _posture;
}