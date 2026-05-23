using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "Player Data", menuName = "ScriptableObjects/Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public static Vector3 LastGroundLocation { get; set; }

    private void OnEnable()
    {
        DefaultPostProcProfile = defaultPostProcProfile;
        PlayMode.OnEnterPlayMode += Init;
        Init();
    }
    void Init()
    {
        CarriedObject = null;
    }
    [Dependency][SerializeField] PostProcessProfile defaultPostProcProfile;
    public static PostProcessProfile DefaultPostProcProfile { get; private set; }

    public static Vector3 Position { get; private set; }
    public static Quaternion Rotation { get; private set; }
    public static void RecordPositionRotation(Vector3 pos, Quaternion rot) { Position = pos; Rotation = rot; }
    public static float SprocketCharge { get; set; }
    public static float SprocketDistance { get; set; }
    public static Transform UIHandUp { get; set; }
    public static Transform UIHandDown { get; set; }

    public static PlayerCarryableObject CarriedObject { get; set; }
}