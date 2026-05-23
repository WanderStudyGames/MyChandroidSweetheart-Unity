using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/TabletProfile", fileName = "Tablet Profile")]
[ProfileFor(typeof(Tablet))]
public class TabletProfile : PlayerAbilityProfile
{
}
