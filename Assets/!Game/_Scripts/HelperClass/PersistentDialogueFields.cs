using UnityEngine;

[System.Serializable]
public struct PersistentDialogueFields
{
    [SerializeField] private Camera _camera;
    public Camera Camera => _camera;
    [SerializeField] private GameObject _gameObject;
    public GameObject GameObject => _gameObject;
    public PersistentDialogueFields(Camera camera = null, GameObject gameObject = null)
    {
        _camera = camera;
        _gameObject = gameObject;
    }
}