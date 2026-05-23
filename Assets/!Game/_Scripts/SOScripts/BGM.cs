using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "BGM", menuName = "ScriptableObjects/Audio/BGM")]
public class BGM : ScriptableObject
{
    public AudioClip track;
    public AudioMixerGroup mixerGroup;
    public float volume;
    public float startDelay;
    public Vector2 fadeTime;
    public float timeToResume = 30f;
}
