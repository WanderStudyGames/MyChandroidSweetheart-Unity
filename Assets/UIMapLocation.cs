using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMapLocation : MonoBehaviour
{
    [SerializeField] private SceneAssetReference[] _scenes;
    public SceneAssetReference[] Scenes => _scenes;
    [SerializeField] private GameObject _false;
    [SerializeField] private GameObject _true;

    [SerializeField] private RectTransform _playerObject;
    [SerializeField] private RectTransform _location;
    private void Awake()
    {
        bool b = false;
        var name = SceneManager.GetActiveScene().name;
        foreach (var scene in _scenes)
        {
            if (WorldData.DiscoveredRooms.Has(scene.SceneName))
            {
                if (scene.SceneName == name) { _playerObject.transform.position = _location.position; }
                b = true;
            }
        }
        if (_false != null)
            _false.SetActive(!b);
        if (_true != null)
            _true.SetActive(b);
    }
}
