using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Music Manager", menuName = "Music Manager")]
public class MusicManager : ScriptableObject
{
    [SerializeField] private GameObject MusicAudioSourcePF;
    [SerializeField] private AudioMixer _audioMixer;
    private static List<MusicFader> _musicFaders = new();
    private static MusicFader _musicSource;
    private static MusicManager _instance;

    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Init;
        Init();

    }
    void Save() { }
    void Init()
    {
        _musicFaders.Clear();
        _instance = this;
    }
    public static void KillBGM()
    {
        PlayBGM(null);
    }

    public static void PlayBGM(BGM bgm)
    {
        if (bgm == null)
        {
            foreach (var fader in _musicFaders)
            {
                fader.Stop();
                Debug.Log("killing track: " + fader.gameObject.GetComponent<AudioSource>().clip.name);
            }
        }
        else
        {
            bool found = false;
            foreach (var fader in _musicFaders)
            {
                if (fader.BGM != bgm)
                {
                    fader.Stop();
                    Logger.LogSigned(nameof(MusicManager), "BGM silenced", new(color: "grey"));
                }
                else
                {
                    fader.Play(bgm);
                    found = true;
                }
            }
            if (!found)
            {
                GameObject go = new("Music");
                AudioSource a = go.AddComponent<AudioSource>();
                MusicFader fader = go.AddComponent<MusicFader>();
                a.loop = true;
                a.bypassReverbZones = true;
                a.spatialBlend = 0;
                a.volume = 0;
                a.outputAudioMixerGroup = bgm.mixerGroup;
                DontDestroyOnLoad(go);
                fader.Play(bgm);
                _musicFaders.Add(fader);
            }
        }
    }
}
