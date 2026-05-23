using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using VInspector;

public class SceneBoolFork : MonoBehaviour
{
    [SerializeField] private bool _useCurrentScene;
#if UNITY_EDITOR
    [SerializeField, HideIf("_useCurrentScene")] private SceneAsset _puzzleScene;
    private void OnValidate()
    {
        if (_puzzleScene != null)
            _sceneName = _puzzleScene.name;
        else if (PrefabUtility.IsPartOfPrefabInstance(this)) Debug.LogWarning("SceneAsset not assigned", gameObject);
    }
#endif
    [SerializeField] private string _sceneName;
    [SerializeField, EndIf] private UnityEvent _true;
    [SerializeField] private UnityEvent _false;
    private void Awake()
    {
        if (_useCurrentScene) _sceneName = SceneManager.GetActiveScene().name;
    }
    private void Start()
    {
        Check();
    }
    private void Check()
    {
        bool b = WorldData.SceneBools.Has(_sceneName);
        if (b) _true.Invoke();
        else _false.Invoke();
    }
    public void SaveTrue()
    {
        WorldData.SceneBools.Add(_sceneName);
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up, _sceneName);
#endif
    }
}
