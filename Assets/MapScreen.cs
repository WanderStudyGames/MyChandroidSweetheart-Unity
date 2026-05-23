using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MapScreen : MonoBehaviour
{
    private static string _bookmarkedSceneName = string.Empty; // Static variable to store the bookmarked scene name
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Init()
    {
        SaveSystem.OnLoadFile += Load;
        SaveSystem.OnSaveFile += Save;
        Load(SaveSystem.Files);
    }

    private static void Load(SaveSystem.SaveFileNames saveFileNames)
    {
        _bookmarkedSceneName = ES3.Load("bookmarkedScene", saveFileNames.worldData, string.Empty);
    }

    private static void Save(SaveSystem.SaveFileNames saveFileNames)
    {
        ES3.Save("bookmarkedScene", _bookmarkedSceneName, saveFileNames.worldData);
    }


    [SerializeField] private ScrollToTransform _scrollToTransform;
    [SerializeField] private RectTransform _playerMarker;
    [SerializeField] private RectTransform _bookmarkTransform;
    [SerializeField] private bool _focusOnEnable = true;
    [Tooltip("Optional cover to hide the map while loading. If set, it will be activated when focusing on a scene and deactivated after scrolling to the node.")]
    [SerializeField] private GameObject _mapCover; // Optional cover to hide the map while loading
    [SerializeField] private SFX _pageChangeSFX; // Optional sound effect for page changes
    [SerializeField] private SFX _bookmarkSFX; // Optional sound effect for bookmarking
    [SerializeField] private SFX _unbookmarkSFX; // Optional sound effect for unbookmarking
    [SerializeField] private UnityEvent _onPageChange;
    [SerializeField] private TMP_Text _pageTitleText;
    [SerializeField] private TMP_Text _nodeTitleText;
    private MapPage[] _mapPages;
    private MapPage _currentPage;
    private void Awake()
    {
        _mapPages = gameObject.GetComponentsFromImmediateChildren<MapPage>(true);
        _mapCover.SetActive(true);

    }
    public void Bookmark(MapNode node)
    {
        if (node == null || node.Scenes == null || node.Scenes.Length <= 0)
        {
            Debug.LogWarning("Cannot bookmark a node with no scenes.");
            return;
        }
        _bookmarkTransform.gameObject.SetActive(false);
        if (_bookmarkedSceneName == node.Scenes[0].SceneName)
        {
            if (_unbookmarkSFX != null) { _unbookmarkSFX.PlayAtPoint(transform.position); }
            _bookmarkedSceneName = string.Empty; // Unbookmark if already bookmarked
            return;
        }
        _bookmarkedSceneName = node.Scenes[0].SceneName; // Bookmark the first scene of the node
        ParentToNode(node, _bookmarkTransform);
        _bookmarkTransform.gameObject.SetActive(true);
        if (_bookmarkSFX != null) { _bookmarkSFX.PlayAtPoint(transform.position); }

    }
    public void HighLight(MapNode node)
    {
        _nodeTitleText.text = node.Name;
    }
    public void UnHighLight()
    {
        _nodeTitleText.text = string.Empty;
    }
    private MapNode FindNodeForScene(string sceneName, out MapPage mapPage, bool recursive = true)
    {
        foreach (var page in _mapPages)
        {
            if (page.TryGetMapNodeForScene(sceneName, out MapNode node, recursive))
            {
                mapPage = page;
                return node;
            }
        }
        mapPage = null;
        return null;
    }
    private void OnEnable()
    {
        foreach (var page in _mapPages)
        {
            page.GetReferences();
        }

        _mapCover?.SetActive(true);

        StartCoroutine(Co_OnEnable());

        IEnumerator Co_OnEnable()
        {
            yield return null;

            if (!_focusOnEnable) yield break;
            yield return null;
            FocusCurrentScene(false);
        }
    }
    public void FocusCurrentScene(bool playSFX = true)
    {
        Debug.Log($"Focusing on current scene: {SceneManager.GetActiveScene().name}");
        FocusScene(SceneManager.GetActiveScene().name, playSFX);
    }
    public void FocusScene(SceneAssetReference sceneReference)
    {
        FocusScene(sceneReference.SceneName);
    }
    public void FocusScene(string sceneName, bool playSFX = true)
    {
        var node = FindNodeForScene(sceneName, out MapPage mapPage, false);
        if (node != null)
        {
            Debug.Log($"Found map node for scene: {sceneName} - {node.gameObject.name}");
            FocusNode(node, mapPage);
            if (playSFX && _pageChangeSFX != null) { _pageChangeSFX.PlayAtPoint(transform.position); }
            _onPageChange?.Invoke();
            _currentPage = mapPage;
            _pageTitleText.text = _currentPage.Name;
        }
        else
        {

            Debug.LogWarning($"No map node found for scene: {sceneName}");
        }

    }
    public void FocusNode(MapNode mapNode, MapPage mapPage)
    {
        Debug.Log($"Focusing on map node: {mapNode.gameObject.name} in page: {mapPage.Name}");
        _mapCover?.SetActive(true);
        StartCoroutine(Co_FocusNode());
        IEnumerator Co_FocusNode()
        {
            yield return null;
            foreach (var page in _mapPages)
            {
                page.gameObject.SetActive(page == mapPage);
                page.ResetSearched();
            }
            yield return null;
            _scrollToTransform.ScrollToTarget(mapNode.GetComponent<RectTransform>());
            ParentToNode(mapNode, _playerMarker);

            // Set the bookmark position
            _bookmarkTransform.gameObject.SetActive(false);
            if (mapPage.TryGetMapNodeForScene(_bookmarkedSceneName, out MapNode bookmarkNode))
            {
                ParentToNode(bookmarkNode, _bookmarkTransform);
                _bookmarkTransform.gameObject.SetActive(true);
            }
            //reset map searches
            foreach (var page in _mapPages) { page.ResetSearched(); }

            yield return null;
            _mapCover?.SetActive(false);
            UnHighLight();
        }
    }
    public void GoToPage(int pageIndex)
    {
        for (int i = 0; i < _mapPages.Length; i++)
        {
            _mapPages[i].gameObject.SetActive(i == pageIndex);
        }
        if (_pageChangeSFX != null) { _pageChangeSFX.PlayAtPoint(transform.position); }
        _onPageChange?.Invoke();
        _currentPage = _mapPages[pageIndex];
        _pageTitleText.text = _currentPage.Name;
        FocusForPage(_mapPages[pageIndex]);
    }
    public void GoToPage(MapPage mapPage)
    {
        for (int i = 0; i < _mapPages.Length; i++)
        {
            _mapPages[i].gameObject.SetActive(_mapPages[i] == mapPage);
        }
        if (_pageChangeSFX != null) { _pageChangeSFX.PlayAtPoint(transform.position); }
        _onPageChange?.Invoke();
        _currentPage = mapPage;
        _pageTitleText.text = _currentPage.Name;
        //get the node for the current scene and focus on it
        FocusForPage(mapPage);
    }
    public void FocusForPage(MapPage mapPage)
    {
        if (mapPage.TryGetMapNodeForScene(SceneManager.GetActiveScene().name, out MapNode node, true))
        {
            Debug.Log($"Focusing on node for current scene: {node.gameObject.name} in page: {mapPage.Name}");
            FocusNode(node, mapPage);
        }
    }

    public void ParentToNode(MapNode node, RectTransform childTransform)
    {
        childTransform.SetParent(transform, true);
        childTransform.localScale = Vector3.one;
        childTransform.localRotation = Quaternion.identity;
        childTransform.SetParent(node.transform.parent, true);
        var tr = node.LocationTransform != null ? node.LocationTransform : node.transform;
        childTransform.position = tr.position;
    }

    public void GoToParent()
    {
        if (_currentPage == null || _currentPage.Parent == null)
        {
            Debug.LogWarning("No parent page to go to.");
            return;
        }
        GoToPage(_currentPage.Parent);
    }
    //activate the correct tab
    //zoom in on the correct node
    //place the player at the correct node
}
