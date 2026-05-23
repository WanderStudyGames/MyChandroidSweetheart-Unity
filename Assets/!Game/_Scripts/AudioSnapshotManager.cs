using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/AudioSnapshotManager", fileName = "Audio Snapshot Manager")]
public class AudioSnapshotManager : ScriptableObject
{
    private static AudioSnapshotManager instance;
    [SerializeField] private AudioMixerSnapshot _defaultSnapshot;
    public static AudioMixerSnapshot DefaultSnapshot => instance._defaultSnapshot;
    [SerializeField] private float _transitionDuration = 0.2f;

    private void OnEnable()
    {
        instance = this;
    }
    public static void SetSnapshot(AudioMixerSnapshot snapshot)
    {
        if (instance == null) return;
        if (snapshot == null) snapshot = instance._defaultSnapshot;
        snapshot.TransitionTo(instance._transitionDuration);
    }

}
