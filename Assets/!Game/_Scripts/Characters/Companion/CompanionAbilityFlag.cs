using UnityEngine;

[System.Serializable]
public class CompanionAbilityFlag : ComponentFlag
{
    [SerializeField] private CompanionComponentProfile profile;
    public override ComponentProfile Profile => profile;

    public CompanionAbilityFlag(bool enabled, CompanionComponentProfile componentProfile) : base(enabled, componentProfile)
    {
        profile = componentProfile;
    }
}
