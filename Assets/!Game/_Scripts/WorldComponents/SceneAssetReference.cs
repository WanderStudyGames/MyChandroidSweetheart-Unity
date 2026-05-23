#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[System.Serializable]
public class SceneAssetReference : ISerializationCallbackReceiver
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset _asset;
    public SceneAsset Asset => _asset;
#endif

    [SerializeField] private string _sceneName = string.Empty;
    public string SceneName { get { return _sceneName; } }


#if UNITY_EDITOR
    public void SetSceneAsset(SceneAsset sceneAsset) { _asset = sceneAsset; UpdateName(); }
    private void UpdateName()
    {
        if (_asset != null)
            _sceneName = _asset.name;
        else _sceneName = string.Empty;

    }
#endif
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        UpdateName();
#endif
    }
    public void OnAfterDeserialize() { }

}
