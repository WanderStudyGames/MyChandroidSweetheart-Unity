using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ControlRebindManager", menuName = "ScriptableObjects/ControlRebindManager")]
public class ControlRebindManager : ScriptableObject
{
    [SerializeField] private InputActionAsset _inputActions;
    private void OnEnable()
    {
        SaveSystem.OnSaveStatic += Save;
        PlayMode.OnEnterPlayMode += Init;
        Init();
    }
    private void Init()
    {
        var data = ES3.Load("rebinds", "settings.es3", "");
        if (string.IsNullOrEmpty(data)) { Debug.LogError($"No rebinds found: {data}"); return; }
        Debug.Log($"Rebinds found: {data}");
        _inputActions.LoadBindingOverridesFromJson(data);

    }
    private void Save()
    {
        if (Application.isPlaying)
        {
            ES3.Save("rebinds", _inputActions.SaveBindingOverridesAsJson(), "settings.es3");
        }
    }

}
