using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "ScriptableObjects/Audio/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    [SerializeField] private AudioMixer audioMixer;
    public AudioSettingsData Data { get; private set; }

    private void OnEnable()
    {
        SaveSystem.OnSaveStatic += Save;
        PlayMode.OnEnterPlayMode += Init;
        SceneStartup.OnSceneStart += SetInitialValues;
        Init();
    }
    private void Init() { Data = ES3.Load("audioSettings", "settings.es3", new AudioSettingsData()); Apply(); }
    private void Save() { ES3.Save("audioSettings", Data, "settings.es3"); }
    private void SetInitialValues()
    {
        Apply();
#if !UNITY_EDITOR
        SceneStartup.OnSceneStart -= SetInitialValues;
#endif
    }

    public class AudioSettingsData
    {
        public float MasterVolume = 80;
        public float BGMVolume = 74;
        public float SFXVolume = 80;
        public float VOXVolume = 80;
    }
    private void Apply()
    {
        bool b = audioMixer.SetFloat("MasterVolume", Data.MasterVolume - 80);

        audioMixer.SetFloat("BGMVolume", Data.BGMVolume - 80);
        audioMixer.SetFloat("SFXVolume", Data.SFXVolume - 80);
        audioMixer.SetFloat("DRYSFXVolume", Data.SFXVolume - 80);
        audioMixer.SetFloat("VOXVolume", Data.VOXVolume - 80);
    }
    public void SetMasterVolume(float volume) { Data.MasterVolume = Mathf.Clamp(volume, 0, 100); Apply(); }
    public void SetBGMVolume(float volume) { Data.BGMVolume = Mathf.Clamp(volume, 0, 100); Apply(); }
    public void SetSFXVolume(float volume) { Data.SFXVolume = Mathf.Clamp(volume, 0, 100); Apply(); }
    public void SetVOXVolume(float volume) { Data.VOXVolume = Mathf.Clamp(volume, 0, 100); Apply(); }
}
