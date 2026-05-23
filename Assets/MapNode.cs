using UnityEngine;

public class MapNode : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Transform LocationTransform { get; private set; }
    [SerializeField] private SceneAssetReference[] _scenes;
    [SerializeField] private MapPage _goToPage;
    [Tooltip("If true, this node will not be considered in recursive searches for discovered nodes.")]
    [SerializeField] private bool _excludeFromPageSearch = false;
    public bool ExcludeFromPageSearch => _excludeFromPageSearch;
    private MapPage _parentPage;
    public SceneAssetReference[] Scenes => _scenes;
    private void Start()
    {
        if (!NodeDiscovered())
        {
            gameObject.SetActive(false);
            return;
        }
    }
    private void Reset()
    {
        if (TryGetComponent(out UIMapLocation location))
        {
            _scenes = location.Scenes;
            DestroyImmediate(location);
        }
    }
    public bool NodeDiscovered()
    {
        foreach (var scene in _scenes)
        {
            if (WorldData.DiscoveredRooms.Has(scene.SceneName))
            {
                return true;
            }
        }
        if (_goToPage != null && _goToPage.PageDiscovered(this))
        {
            return true;
        }
        return false;
    }
    public void GoToPage()
    {
        if (_parentPage != null)
            _parentPage.GoToPage(_goToPage);
    }
    private void Awake()
    {
        _parentPage = GetComponentInParent<MapPage>(true);
    }
    public bool Contains(MapNode node)
    {
        if (node == null) return false;
        if (this == node) return true;
        if (_goToPage != null && !_goToPage.Searched && _goToPage.Has(node))
        {
            return true;
        }
        return false;
    }
    public bool Contains(string sceneName, bool recursive = true)
    {
        foreach (var s in _scenes)
        {
            if (s.SceneName.Equals(sceneName))
            {
                return true;
            }
        }
        if (recursive && _goToPage != null && !_goToPage.Searched && _goToPage.Has(sceneName))
        {
            return true;
        }
        return false;
    }

    public void Bookmark()
    {
        _parentPage?.Bookmark(this);
    }

    public void HighLight() { _parentPage?.HighLight(this); }
    public void UnHighLight() { _parentPage?.UnHighLight(); }

    [System.Serializable]
    public class MapNodeConnection
    {
        [field: SerializeField] public MapNode Node { get; private set; }
        [field: SerializeField] public GameObject[] Pellets { get; private set; }
    }
}