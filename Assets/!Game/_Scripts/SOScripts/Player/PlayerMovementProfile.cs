using UnityEngine;

[CreateAssetMenu(fileName = "Player Movement Profile", menuName = "ScriptableObjects/Player/Player Movement Profile")]
public class PlayerMovementProfile : PlayerComponentProfile
{
    [Range(0, 10)] public float accelerationGround = 3.5f;
    [Range(0, 10)] public float accelerationAir = 0.6f;
    [Range(0, 10)] public float sprintSpeed = 5f;
    [Range(0, 1)] public float swimSpeedMultiplier = 0.75f;
    [Range(0, 10)] public float walkSpeed = 3.5f;
    [Range(0.25f, 0.45f)] public float friction = 0.35f;
    [Range(0.1f, 1f)] public float gravity = 0.2f;
    [Range(0.07f, 0.5f)] public float jumpForce = 0.08f;
    [Range(0, 20)] public float fallDamageThreshhold = 10f;
    [Range(0, 40)] public float fallDamageThreshholdSecondary = 10f;
    public Sprite fallDeathSprite;
    public SFX concreteStepSFX;
    public SFX concreteLandSFX;
    public SFX concreteJumpSFX;
    public SFX swimSFX;
    public SFX splashSFX;
    public SFX fallingSFX;
    public SFX fallDamageSFX;
    public SFX drownSFX;
    public SFX jumpComboSFX;
    public GameEvent OnEnterWaterGE;
    public GameEvent OnExitWaterGE;
    public InventoryItemMetadata PreventFallDamageItem;
    public InventoryItemMetadata PreventFallDamageItemSecondary;
    public InventoryItemMetadata PreventDrownItem;
    [Range(0, 100)] public float stepDistance = 2.2f;
    [Range(0, 100)] public float swimStrokeDistance = 4f;
    [Range(0, 2)] public float landingSFXVelocityThreshhold;
    public AnimationCurve cameraAnimCurve;
    public AnimationCurve fallingVolumeAnimCurve;
    public LayerMask groundLayers;
    public GameObject longFallParticlePrefab;
    public float largeFallThreshhold;
}
