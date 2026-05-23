using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FileSelectButton : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private int fileNumber;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _companionName;
    [SerializeField] private TMP_Text _and;
    [SerializeField] private bool _updateDisplayOnEnable = true;
    [SerializeField] private UnityEvent<int> _onCheckFile;
    [SerializeField] private UnityEvent<int> _onLoad;
    void OnEnable()
    {
        if (_updateDisplayOnEnable)
            CheckFile(fileNumber);
    }
    public void CheckFile(int i)
    {
        if (_playerName != null)
            _playerName.text = ES3.Load<string>(key: "name", filePath: $"playerData{i}.es3", defaultValue: null);
        if (_companionName != null)
            _companionName.text = ES3.Load<string>(key: "name", filePath: $"companionData{i}.es3", defaultValue: null);
        if (_and != null)
            _and.gameObject.SetActive(_playerName?.text != null && _companionName?.text != null);
        _onCheckFile.Invoke(i);
    }

    public void LoadFile()
    {
        SaveSystem.Load(fileNumber);
        _onLoad.Invoke(fileNumber);
    }

}
