using UnityEngine;
using UnityEngine.UI;

public class ReadAudioSettings : MonoBehaviour
{
    [SerializeField] private AudioSettings _settings;
    [SerializeField] private Slider _masterVolume;
    [SerializeField] private Slider _BGMVolume;
    [SerializeField] private Slider _SFXVolume;
    [SerializeField] private Slider _VoxVolume;
    private void OnEnable()
    {
        _masterVolume.value = _settings.Data.MasterVolume;
        _BGMVolume.value = _settings.Data.BGMVolume;
        _BGMVolume.onValueChanged.Invoke(_BGMVolume.value);
        _SFXVolume.value = _settings.Data.SFXVolume;
        _SFXVolume.onValueChanged.Invoke(_SFXVolume.value);
        _VoxVolume.value = _settings.Data.VOXVolume;
        _VoxVolume.onValueChanged.Invoke(_VoxVolume.value);
    }
}
