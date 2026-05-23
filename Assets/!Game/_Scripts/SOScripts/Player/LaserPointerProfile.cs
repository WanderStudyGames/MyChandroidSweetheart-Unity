using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/LaserPointerProfile", fileName = "Laser Pointer Profile")]
[ProfileFor(typeof(LaserPointer))]
public class LaserPointerProfile : PlayerAbilityProfile
{
    [field: SerializeField] public LayerMask LayerMask { get; private set; }
}
