using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAbilityFlag : ComponentFlag
{
    [SerializeField] private PlayerAbilityProfile _abilityProfile;
    public PlayerAbilityProfile AProfile => _abilityProfile;
    public PlayerAbilityFlag(bool enabled, PlayerAbilityProfile abilityProfile) : base(enabled, abilityProfile)
    {
        _abilityProfile = abilityProfile;
    }
}
