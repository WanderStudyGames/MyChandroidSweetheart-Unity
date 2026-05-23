using UnityEngine;

[CreateAssetMenu(fileName = "Sprocket Profile", menuName = "ScriptableObjects/Player/Sprocket Profile")]
[ProfileFor(typeof(Sprocket))]
public class SprocketProfile : PlayerAbilityProfile
{
    public float maxLaunchForce = 0.19f;
    [Range(0.001f, 0.01f)] public float chargeSpeed = 0.008f;
    [Range(0, 1)] public float sprocketingWalkSpeedMultiplier = 0.25f;

    public AnimationCurve PressureProfile
    {
        get
        {
            return new()
            {
                keys = new Keyframe[]
                {
                    new() {
                        time = 1.68f,
                        value = 1,
                        inTangent = 0,
                        outTangent = 0,
                        inWeight = 0,
                        outWeight = 0,
                        //tangentMode = 0,
                        weightedMode = WeightedMode.None
                    },
                    new() {
                        time = 3f,
                        value = 0,
                        inTangent = -0.3994565f,
                        outTangent = 0,
                        inWeight = 0.7104779f,
                        outWeight = 0,
                        //tangentMode = 1,
                        weightedMode = WeightedMode.In
                    }
                },
                preWrapMode = WrapMode.ClampForever,
                postWrapMode = WrapMode.ClampForever
            };
        }
    }

    public LayerMask layerMask = 192;
    public string physicsLayer = "phys";
    public float physicsPushMultiplier = 300f;
    [Range(0, 1)] public float littleChargeCutoff;
    [Range(0, 1)] public float bigChargeCutoff;
    public SFX BigLaunchSFX;
    public SFX LittleLaunchSFX;
    public SFX ChargeSFX;
    public SFX ChargeJumpSFX;
    public SFX FailSFX;
    public SFX ImpactSFXBig;
    public SFX ImpactSFXSmall;
    public SFX SprocketLaunchJumpBig;
    public SFX SprocketLaunchJumpSmall;
    public ParticleSystem impactParticlesBig;
    public ParticleSystem impactParticlesSmall;
    public InventoryItemMetadata sprocketJumpItem;
}
