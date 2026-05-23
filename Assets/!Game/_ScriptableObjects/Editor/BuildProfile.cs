using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "BuildProfile", menuName = "ScriptableObjects/BuildProfile")]
public class BuildProfile : ScriptableObject
{
    [System.Serializable]
    private class SerializedBuildPlayerOptions
    {
        [SerializeField] private BuildTarget _target;
        [SerializeField] private string _path;
        public BuildPlayerOptions GetOptions()
        {
            return new BuildPlayerOptions()
            {
                target = _target,
                locationPathName = _path
            };
        }
    }
    [SerializeField] private string _version;
    [SerializeField] private SceneAssetReference[] _scenes;
    [SerializeField] private SaveDataSnapshotProvider _saveDataSnapshotProvider;
    [SerializeField] private SaveDataSnapshot[] _saveDataSnapshots;
    [SerializeField] private bool _showSaveDataOnMainMenu = false;
    private List<SerializedBuildPlayerOptions> _buildOptions;
    [Button("Copy Scenes from Build List", color = "Red")]
    private void GetScenes()
    {
        List<SceneAssetReference> list = new();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            var sar = new SceneAssetReference();
            sar.SetSceneAsset(AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path));
            list.Add(sar);
        }
        _scenes = list.ToArray();
    }
    [Button("Send Data to Build Settings", color = "Green")]
    private void PushScenes()
    {
        List<EditorBuildSettingsScene> list = new();
        foreach (var sar in _scenes)
        {
            list.Add(new() { path = AssetDatabase.GetAssetPath(sar.Asset), enabled = true });
        }
        EditorBuildSettings.scenes = list.ToArray();
        PlayerSettings.bundleVersion = _version;
        _saveDataSnapshotProvider.SetSnapshots(_saveDataSnapshots);
        _saveDataSnapshotProvider.SetShowOnMainMenu(_showSaveDataOnMainMenu);
    }
    private void PushAndBuild()
    {
        //set build in motion
        //building = true
    }
    private BuildReport _buildReport;

    [UnityEditor.Callbacks.PostProcessBuild]
    private static void AfterBuild(BuildTarget target, string thing)
    {

        //if building is true
        //move down the list
        //if end of list || build errored
        //building = false
    }
    private void StartBuild(BuildPlayerOptions buildPlayerOptions)
    {
        _buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
    private bool BuildErrored()
    {
        return _buildReport != null && _buildReport.summary.totalErrors > 0;
    }
}