using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
public class DialogueComponentsProvider : MonoBehaviour, IDialogueComponents
{
    [Header("Persistent Data Target")]
    [SerializeField] private DialogueComponents _componentsObject;
    [Header("Components")]
    [SerializeField] private Camera _camera;
    public Camera Camera => _camera;
    [FormerlySerializedAs("focusObject")]
    [SerializeField] private Transform _focusObject;
    public Transform FocusObject => _focusObject;
    [ContextMenu("Assign")]
    private IEnumerator Start()
    {
        if (_camera != null)
            _camera.enabled = false;
        if (_componentsObject == null) yield break;
        yield return null;
        _componentsObject.FocusObject = _focusObject;
        _componentsObject.Camera = _camera;
    }
}
