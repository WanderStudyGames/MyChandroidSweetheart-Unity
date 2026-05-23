using TMPro;
using UnityEngine;

public class FileStatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _companionName;
    [SerializeField] private TMP_Text _scans;
    [SerializeField] private TMP_Text _rooms;
    [SerializeField] private TMP_Text _batteries;
    private void OnEnable()
    {
        if (_playerName != null)
            _playerName.text = ES3.Load<string>(key: "name", filePath: $"playerData{SaveSystem.SaveFile}.es3", defaultValue: null);
        if (_companionName != null)
            _companionName.text = ES3.Load<string>(key: "name", filePath: $"companionData{SaveSystem.SaveFile}.es3", defaultValue: null);
        if (_scans != null)
        {
            _scans.text = WorldData.CompletedScans.GetCount().ToString();
            if (_scans.text.Length == 1) { _scans.text = "0" + _scans.text; }
        }
        if (_rooms != null)
        {
            _rooms.text = WorldData.DiscoveredRooms.GetCount().ToString();
            if (_rooms.text.Length == 1) { _rooms.text = "0" + _rooms.text; }
        }
        if (_batteries != null)
        {
            _batteries.text = WorldData.SceneBools.GetCount().ToString();
            if (_batteries.text.Length == 1) { _batteries.text = "0" + _batteries.text; }
        }
    }
}
