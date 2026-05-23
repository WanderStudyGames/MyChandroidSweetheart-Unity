using QFSW.QC;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveDataSnapshot")]
public class SaveDataSnapshot : ScriptableObject
{
    [SerializeField] private string _label;
    [SerializeField] private SaveSystem.SaveFileNames _fileNames;
    [SerializeField, TextArea] private string _materialData = "";
    [SerializeField, TextArea] private string _companionData = "";
    [SerializeField, TextArea] private string _playerData = "";
    [SerializeField, TextArea] private string _worldData = "";
    public string Label => _label;
    private void OnEnable()
    {
        _fileNames = new()
        {
            materialData = "snapshotMaterialData.es3",
            companionData = "snapshotCompanionData.es3",
            playerData = "snapshotPlayerData.es3",
            worldData = "snapshotWorldData.es3"
        };
    }
    [Button]
    public void SaveData()
    {
        if (!Application.isPlaying) return;
        SaveSystem.SaveSnapshot(_fileNames);
        _materialData = ES3.LoadRawString(_fileNames.materialData);
        _companionData = ES3.LoadRawString(_fileNames.companionData);
        _playerData = ES3.LoadRawString(_fileNames.playerData);
        _worldData = ES3.LoadRawString(_fileNames.worldData);
    }
    [Button]
    public void LoadData()
    {
        if (!Application.isPlaying) return;
        SaveSystem.Save();
        ES3.SaveRaw(_materialData, _fileNames.materialData);
        ES3.SaveRaw(_companionData, _fileNames.companionData);
        ES3.SaveRaw(_playerData, _fileNames.playerData);
        ES3.SaveRaw(_worldData, _fileNames.worldData);
        SaveSystem.LoadSnapshot(_fileNames);
        SceneHandler.LoadLoadedScene();
        //SceneHandler.FadeThenResetScene();
    }
    [Command("snapshot-load")]
    private static string LoadSnapshot([SnapshotTag] string name)
    {
        Resources.Load<SaveDataSnapshot>(name).LoadData();
        return $"Loaded snapshot {name}";
    }
    public struct SnapshotTag : IQcSuggestorTag { }
    public sealed class SnapshotTagAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags = { new SnapshotTag() };
        public override IQcSuggestorTag[] GetSuggestorTags()
        {
            return _tags;
        }
    }
    public class SnapshotSuggestor : BasicCachedQcSuggestor<SaveDataSnapshot>
    {
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<SnapshotTag>();
        }
        protected override IQcSuggestion ItemToSuggestion(SaveDataSnapshot snapshot)
        {
            return new RawSuggestion(snapshot.name, true);
        }
        protected override IEnumerable<SaveDataSnapshot> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            return Resources.LoadAll<SaveDataSnapshot>("");
        }
    }
}