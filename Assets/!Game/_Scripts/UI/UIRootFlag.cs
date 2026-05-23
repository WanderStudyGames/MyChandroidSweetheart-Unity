using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class UIRootFlag : ComponentFlag
{
    private readonly PlayerAbilityFlag[] _playerAbilities = { };
    [SerializeField] UIRootProfile _UIRootProfile;
    public override bool Enabled { get {
            foreach(PlayerAbilityFlag p in _playerAbilities)
            {
                //need something to check against for determining whether UI element is associated with player ability
                if(p.Profile == _UIRootProfile) { return p.Enabled; }
            }
            return false;

        } }
    public UIRootFlag(bool enabled, ComponentProfile componentProfile) : base(enabled, componentProfile)
    {
    }
}
