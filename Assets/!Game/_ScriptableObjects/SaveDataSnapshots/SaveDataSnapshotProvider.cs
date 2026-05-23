using UnityEngine;

[CreateAssetMenu(fileName = "Save Data Snapshot Provider", menuName = "ScriptableObjects/SaveDataSnapshotProvider")]
public class SaveDataSnapshotProvider : ScriptableObject
{
    public SaveDataSnapshot[] Snapshots => _snapshots;
    [SerializeField, VInspector.ReadOnly] private SaveDataSnapshot[] _snapshots;
    public void SetSnapshots(SaveDataSnapshot[] snapshots)
    {
        _snapshots = snapshots;
    }
    [SerializeField, VInspector.ReadOnly] private bool _showOnMainMenu = false;
    public bool ShowOnMainMenu => _showOnMainMenu;
    public void SetShowOnMainMenu(bool show)
    {
        _showOnMainMenu = show;
    }
    public static SaveDataSnapshotProvider Instance;
    private void OnEnable()
    {
        Instance = this;
        Debug.Log("SaveDataSnapshotProvider enabled");
    }
}