using UnityEngine;
using UnityEngine.Audio;
using VInspector;

[CreateAssetMenu(fileName = "SFX", menuName = "ScriptableObjects/Audio/SFX")]
public class SFX : ScriptableObject
{

    [SerializeField] private AudioClip[] clips;
    public AudioClip Clip
    {
        get
        {
            if (clips.Length == 0) return null;
            int rand = Random.Range(0, clips.Length);
            if (rand == clips.Length) rand = clips.Length - 1;
            return clips[rand];
        }
    }
    [Range(0, 1)][SerializeField] private float volume = 0.1f;
    [SerializeField, MinMaxSlider(-0.5f, 0.5f)] private Vector2 pitchVariation;
    public Vector2 PitchVariation => pitchVariation;
    public float Volume => volume;
    [Range(0, 1)][SerializeField] private float spatialBlend;
    public float SpatialBlend => spatialBlend;
    [SerializeField] private bool loop;
    public bool Loop => loop;
    [SerializeField] private bool bypassReverb;
    [SerializeField, Range(0, 360)] private float spread = 0;
    public float Spread => spread;
    public bool BypassReverb => bypassReverb;
    [SerializeField] private AudioMixerGroup mixerGroup;
    public AudioMixerGroup MixerGroup => mixerGroup;

#if UNITY_EDITOR
    public void Init(AudioClip[] audioClips) { clips = audioClips; }
#endif
    /// <summary>
    /// Plays the SFX at a specific point in the world, creating a new, unattached GameObject for the sound source, which will be cleaned up automatically after playback.
    /// <param name="sfx">The sound effect to play.</param>
    /// <param name="point"/>The position in world space where the sound will be played.</param>
    /// </summary>
    public static AudioSource PlayAtPoint(SFX sfx, Vector3 point)
    {
        GameObject sound = new(sfx.name);
        sound.transform.position = point;
        var aSource = sound.AddComponent<AudioSource>();
        aSource.PlaySFX(sfx);
        sound.AddComponent<AudioSourceCleanup>();
        return aSource;
    }
    /// <summary>
    /// Plays the SFX at a specific point in the world, creating a new, unattached GameObject for the sound source, which will be cleaned up automatically after playback.
    /// </summary>
    /// <param name="point">The position in world space where the sound will be played.</param>
    /// <returns> AudioSource which is playing the sound.</returns>
    public AudioSource PlayAtPoint(Vector3 point)
    {
        return PlayAtPoint(this, point);
    }
    public void PlayDetached()
    {
        PlayAtPoint(Vector3.zero);
    }
}
