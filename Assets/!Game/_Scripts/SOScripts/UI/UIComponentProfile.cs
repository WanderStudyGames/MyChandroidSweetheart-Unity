using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponentProfile : ComponentProfile
{
    //[SerializeField] PlayerAbilityProfile _associatedPlayerAbility;
    //public PlayerAbilityProfile AssociatedPlayerAbility => _associatedPlayerAbility;
    [Dependency] [SerializeField] private UIManager uiManager;
    public UIManager UIManager => uiManager;
}
