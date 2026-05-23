using UnityEngine;

[CreateAssetMenu(fileName = "Grapple Profile", menuName = "ScriptableObjects/Player/Grapple Profile")]
[ProfileFor(typeof(Grapple))]
public class GrappleProfile : PlayerAbilityProfile
{
    public LayerMask layerMask = 64;
    //public string targetTagName = "Grappleable";//refactor to have RuntimeSet of grappleable objects that you check against when you grapple
    [Range(0, 100)] public float distanceLimit = 30f;
    public float ropeThickness = 0.1f;
    public Material ropeMaterial;
    public const float SwingSpeedLimit = 100f;
    public const float swingSpeedAmt = 80f;
    public SFX grappleLaunchSFX;
    public SFX grappleSwingSFX;
    public SFX grappleRetractSFX;
    public Sprite grappleActionIcon;
    public AnimationCurve swingAudioCurve;
    [SerializeField] private GameObject _grappleIconPF;
    public static GameObject GrappleIconPF { get; private set; }
    [SerializeField] private SFX grappleIconEngageSFX;
    public static SFX GrappleIconEngageSFX { get; private set; }
    [SerializeField] private Color grappleIconPassiveColor;
    public static Color GrappleIconPassiveColor { get; private set; }
    [SerializeField] private Color grappleIconActiveColor;
    public static Color GrappleIconActiveColor { get; private set; }
    private void OnEnable()
    {
        GrappleIconPF = _grappleIconPF;
        GrappleIconEngageSFX = grappleIconEngageSFX;
        GrappleIconPassiveColor = grappleIconPassiveColor;
        GrappleIconActiveColor = grappleIconActiveColor;
    }
    public AnimationCurve GrappleSpeedProfile
    {
        get
        {
            return new()
            {
                keys = new Keyframe[]
                {
                    new()
                    {
                        time = 0.0f,
                        value = 0.0f,
                        weightedMode = 0,
                        //tangentMode= 1,
                        inTangent = 0f,
                        outTangent = 0f,
                        inWeight = 0.0f,
                        outWeight = 0.0f
                    },
                    new()
                    {
                        time= 0.4993148744106293f,
                        value= 0.4989722967147827f,
                        weightedMode= 0,
                        //tangentMode= 0,
                        inTangent = 1.4511666297912598f,
                        outTangent = 1.4511666297912598f,
                        inWeight= 0.3333333432674408f,
                        outWeight= 0.08581127971410752f
                    },
                    new()
                    {
                        time= 1.0f,
                        value= 0.800000011920929f,
                        weightedMode= 0,
                        //tangentMode= 1,
                        inTangent = 0f,
                        outTangent = 0f,
                        inWeight= 0.0f,
                        outWeight= 0.0f
                    }
                }

            };
        }
    }
    public AnimationCurve GrappleEntryConversionRate
    {
        get
        {
            return new()
            {
                keys = new Keyframe[]
                {
                    new()
                    {
                        time = 0.0f,
                        value = 8.0f,
                        inTangent = -Mathf.Infinity,
                        outTangent = -0.8615236878395081f,
                        //tangentMode = 5,
                        weightedMode = 0,
                        inWeight = 0.0f,
                        outWeight = 0.049632348120212558f
                    },
                    new()
                    {
                        time = 20.0f,
                        value = 0.6000000238418579f,
                        inTangent = -0.057579152286052707f,
                        outTangent = 0.0f,
                        //tangentMode = 65,
                        weightedMode = 0,
                        inWeight = 0.03584565967321396f,
                        outWeight = 0.0f
                    }
                }

            };

        }
    }

    public AnimationCurve GrappleExitConversionRate
    {
        get
        {
            return new()
            {
                keys = new Keyframe[]
                {
                    new()
                    {
                        time = 0.0f,
                        value = 0.0010000000474974514f,
                        inTangent = 0.0010435680160298944f,
                        outTangent = 0.000014950806871638633f,
                        //tangentMode = 5,
                        weightedMode = 0,
                        inWeight = 0.0f,
                        outWeight = 0.11672792583703995f
                    },
                    new()
                    {
                        time = 20.0f,
                        value = 0.003000000026077032f,
                        inTangent = 0.0002454857458360493f,
                        outTangent = 0.0001500000071246177f,
                        //tangentMode = 65,
                        weightedMode = 0,
                        inWeight = 0.05514717102050781f,
                        outWeight = 0.0f
                    }
                }

            };
        }
    }
}
