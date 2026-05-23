using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAbilityProfile : PlayerComponentProfile
{
    [SerializeField] private Texture _icon;
    public Texture Icon => _icon;
}