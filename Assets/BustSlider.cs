using QFSW.QC;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BustSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Slider _buttSlider;
    public static event Action OnValueChanged;
    [RuntimeInitializeOnLoadMethod(loadType: RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void OnInit()
    {
        SaveSystem.OnLoadFile += Load;
        SaveSystem.OnSaveFile += Save;
        Load(SaveSystem.Files);
    }
    private static void Load(SaveSystem.SaveFileNames files)
    {
        Value = ES3.Load("bustSize", files.companionData, 1f);
        ButtValue = ES3.Load("buttSize", files.companionData, 1f);
        OnValueChanged?.Invoke();
    }
    private static void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save("bustSize", Value, files.companionData);
        ES3.Save("buttSize", ButtValue, files.companionData);
    }
    [Command("bust-scale")]
    public static float Value = 1;
    [Command("butt-scale")]
    public static float ButtValue = 1;
    private void OnEnable()
    {
        slider.value = Value;
        _buttSlider.value = ButtValue;
        slider.onValueChanged.Invoke(slider.value);
    }
    public void SetButtValue(float f)
    {
        ButtValue = f;
        OnValueChanged?.Invoke();
    }
    public void SetValue(float f)
    {
        Value = f;
        OnValueChanged?.Invoke();
    }

}
