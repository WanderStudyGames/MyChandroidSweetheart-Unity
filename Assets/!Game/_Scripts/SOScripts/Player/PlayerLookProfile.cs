using UnityEngine;

[CreateAssetMenu(fileName = "PLP_", menuName = "ScriptableObjects/Player/Player Look Profile")]
public class PlayerLookProfile : PlayerComponentProfile
{
    private void OnEnable()
    {
        Debug.Log("Player Look Profile enabled");
        PlayMode.OnEnterPlayMode += Init;
        SaveSystem.OnSaveStatic += Save;
        Init();
    }
    private void Init()
    {
        _lookSpeed = ES3.Load("lookSensitivity", "settings.es3", 0.1f);
        _invertLook = ES3.Load("invertLook", "settings.es3", false);
    }
    private void Save()
    {
        ES3.Save("lookSensitivity", _lookSpeed, "settings.es3");
        ES3.Save("invertLook", _invertLook, "settings.es3");
    }
    public void SetMouseSpeed(float speed) { _lookSpeed = speed; }
    public void SetMouseInvert(bool inverted) { _invertLook = inverted; }
    [Range(0, 200)][SerializeField] private float _lookSpeed = 0.1f;
    public float LookSpeed => _lookSpeed;
    [SerializeField] private bool _invertLook = false;
    public bool InvertLook => _invertLook;
    [SerializeField] private bool _clampXEnabled = false;
    public bool ClampXEnabled => _clampXEnabled;
    [SerializeField] private Vector2 _clampX = new(0f, 0f);
    public Vector2 ClampX => _clampX;
    [SerializeField] private Vector2 _clampY = new(-90f, 90f);
    public Vector2 ClampY => _clampY;

}