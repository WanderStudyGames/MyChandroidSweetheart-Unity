using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "StepSoundBank", menuName = "ScriptableObjects/Audio/StepSoundBank")]
public class StepSoundBank : ScriptableObject
{
    [field: SerializeField, MinMaxSlider(0f, 1f)] public Vector2 StepThreshholds { get; private set; }
    [SerializeField] private StepSound _defaultStepSound = new();
    [SerializeField] private StepSound[] _stepSounds;
    [Button]
    public void SetSFX()
    {
        foreach (var stepSound in _stepSounds)
        {
            stepSound.SetSFX();
        }
    }
    public StepSound GetStepSoundForTag(string tag)
    {

        if (_stepSounds == null) { Logger.Error("WHAT THE FUCK"); }
        foreach (var stepSound in _stepSounds)
        {
            if (stepSound.Tag == tag)
            {
                return stepSound;
            }
        }
        return _defaultStepSound;
    }
    public SFX GetLandSFX(float velocity, string tag)
    {
        SFX sfx;
        var ss = GetStepSoundForTag(tag);
        velocity = Mathf.Abs(velocity);
        if (velocity < StepThreshholds.x)
        {
            sfx = ss.SoftLandSFX;
        }
        else if (velocity < StepThreshholds.y)
        {
            sfx = ss.NormalLandSFX;
        }
        else
        {
            sfx = ss.HardLandSFX;
        }
        return sfx;
    }
}
[System.Serializable]
public class StepSound
{

    [field: SerializeField] public string Tag { get; private set; }
    [field: SerializeField, Header("Sounds")] public SFX StepSFX { get; private set; }
    [field: SerializeField] public SFX JumpSFX { get; private set; }
    [field: SerializeField] public SFX SoftLandSFX { get; private set; }
    [field: SerializeField] public SFX NormalLandSFX { get; private set; }
    [field: SerializeField] public SFX HardLandSFX { get; private set; }
    [field: SerializeField] public VelocitySound[] LandSounds { get; private set; }
    [Button]
    public void SetSFX()
    {
        if (LandSounds.Length > 0)
            SoftLandSFX = LandSounds[0].SFX;
        if (LandSounds.Length > 1)
            NormalLandSFX = LandSounds[1].SFX;
        if (LandSounds.Length > 2)
            HardLandSFX = LandSounds[2].SFX;
    }
    //public SFX GetSFXAtVelocity(float velocity)
    //{
    //    SFX sfx = null;
    //    foreach (VelocitySound sound in LandSounds)
    //    {
    //        sfx = sound.SFX;
    //        if (Mathf.Abs(velocity).FallsBetween(sound.VelocityRange))
    //        {
    //            return sound.SFX;
    //        }
    //    }
    //    return sfx;
    //}
}
[System.Serializable]
public class VelocitySound
{

    [field: SerializeField] public SFX SFX { get; private set; }
    [field: SerializeField] public Vector2 VelocityRange { get; private set; }
}
