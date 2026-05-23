using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JoystickControlPathOverrides", menuName = "ScriptableObjects/JoystickControlPathOverrides")]
public class JoystickControlPathOverrides : ScriptableObject
{
    [SerializeField] private Override[] _overrides;
    private void OnEnable()
    {
        SaveSystem.OnSaveStatic += Save;
        PlayMode.OnEnterPlayMode += Init;
        Init();
    }
    private void Init()
    {
        var data = ES3.Load("joystickOverrides", "settings.es3", new Override[0]);
        if (data.Length == 0) return;
        _overrides = data;
    }
    private void Save()
    {
        if (Application.isPlaying)
            ES3.Save("joystickOverrides", _overrides, "settings.es3");
    }
    public string GetOverride(string s)
    {
        foreach (var o in _overrides)
        {
            if (o.value == s && !string.IsNullOrEmpty(o.value)) { return o.key; }
        }
        return s;
    }
    public string GetOverride(int index)
    {
        if (index < 0 || index >= _overrides.Length) return null;
        return _overrides[index].value;
    }
    public string GetKey(int index)
    {
        if (index < 0 || index >= _overrides.Length) return null;
        return _overrides[index].key;
    }
    public int KeyCount => _overrides.Length;
    public void SetOverrideValue(int index, string value)
    {
        if (index < 0 || index >= _overrides.Length) return;
        _overrides[index].value = value;
    }
    [Serializable]
    public struct Override
    {
        public string key;
        public string value;
    }
}