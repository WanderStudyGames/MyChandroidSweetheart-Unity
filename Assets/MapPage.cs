using UnityEngine;

public class MapPage : MonoBehaviour
{
    [field: SerializeField] public MapPage Parent { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    private MapNode[] _nodes = { };
    private MapScreen _mapScreen;
    public bool Searched { get; private set; } = false; // Indicates if the page has been searched for nodes
    public void ResetSearched() { Searched = false; }
    private void Awake()
    {
        GetReferences();
    }
    public void GetReferences()
    {
        _mapScreen = GetComponentInParent<MapScreen>(true);
        _nodes = gameObject.GetComponentsFromImmediateChildren<MapNode>(true);
    }
    public bool TryGetMapNodeForScene(string sceneName, out MapNode node, bool recursive = true)
    {
        Debug.Log($"Searching for scene '{sceneName}' in page '{Name}' (Recursive: {recursive})");
        if (string.IsNullOrEmpty(sceneName)) { node = null; return false; }
        Searched = true;
        foreach (var n in _nodes)
        {
            if (n.Contains(sceneName, recursive))
            {
                node = n;
                return true;
            }
        }
        Debug.LogWarning($"Scene '{sceneName}' not found in page '{Name}' with recursive: {recursive}");
        node = null;
        return false;
    }
    public bool Has(MapNode node)
    {
        Searched = true;
        foreach (var n in _nodes)
        {
            if (n.Contains(node))
            {
                return true;
            }
        }
        return false;
    }
    public bool Has(string sceneName)
    {
        Searched = true;
        foreach (var n in _nodes)
        {
            if (n.Contains(sceneName))
            {
                return true;
            }
        }
        return false;
    }
    public void GoToPage(MapPage mapPage)
    {
        _mapScreen.GoToPage(mapPage);
    }
    public bool PageDiscovered(MapNode from)
    {
        foreach (var n in _nodes)
        {
            if (n == from) continue; // Skip the node that called this method
            if (!n.ExcludeFromPageSearch && n.NodeDiscovered())
            {
                return true;
            }
        }
        return false;
    }

    public void Bookmark(MapNode node)
    {
        _mapScreen.Bookmark(node);
    }

    public void HighLight(MapNode node)
    {
        _mapScreen.HighLight(node);
    }

    public void UnHighLight()
    {
        _mapScreen.UnHighLight();
    }
}