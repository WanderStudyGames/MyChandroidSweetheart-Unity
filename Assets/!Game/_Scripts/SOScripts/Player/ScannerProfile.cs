using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ScannerProfile", menuName = "ScriptableObjects/Player/ScannerProfile")]
[ProfileFor(typeof(Scanner))]
public class ScannerProfile : PlayerAbilityProfile
{
    [SerializeField] private InteractProfile _interactProfile;

    [field: SerializeField] public InputActionReference SwitchModeAction { get; private set; }
    public InteractProfile InteractProfile => _interactProfile;

    [SerializeField] private LayerMask _layerMask;
    public LayerMask LayerMask => _layerMask;
    public SFX scanStartSFX;
    public SFX scanLeaveSFX;
    public SFX scanModeSwitchOnSFX;
    public SFX scanModeSwitchOffSFX;
    public SFX scanPageSkipSFX;
    public SFX scanAmbienceSFX;

}