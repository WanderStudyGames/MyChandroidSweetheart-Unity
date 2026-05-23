using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "UIManager", menuName = "ScriptableObjects/UIManager")]
public class UIManager : ScriptableObject
{
    private static UIManager instance;
    [BigTitle("UI Manager")]
    [Dependency][SerializeField] private GameObject _prefab;
    public static GameObject Prefab => instance._prefab;
    public static TextAlignmentOptions HeaderAlignment { get; set; }

    public static event Action OnShowHeader;
    public static event Action OnHideHeader;
    private static GameObject _currentGO;

    private static string _headerText;
    public static string HeaderText => _headerText;
    public static void Header(string text) { _headerText = text; OnShowHeader?.Invoke(); }
    public static void HideHeader() { OnHideHeader?.Invoke(); }
    public static void SetUIEnabled(bool b)
    {
        if (_currentGO == null) return;
        _currentGO.SetActive(b);
    }
    public static void SpawnUI()
    {
        _currentGO = GameObjectUtility.SpawnUnique(Prefab);
    }
    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Init;
        SaveSystem.OnSaveStatic += Save;
        instance = this;
        Init();
    }
    public static float VirtualMouseSpeed { get; set; }
    public void SetVirtualMouseSpeed(float speed) { VirtualMouseSpeed = speed; }
    private void Init() { VirtualMouseSpeed = ES3.Load("virtualMouseSpeed", "settings.es3", 500f); }
    private void Save() { ES3.Save("virtualMouseSpeed", VirtualMouseSpeed, "settings.es3"); }

}
