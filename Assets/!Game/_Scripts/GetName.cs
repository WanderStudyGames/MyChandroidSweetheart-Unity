using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
public class GetName : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMesh;

    private void OnValidate()
    {
        if (_textMesh == null)
            _textMesh = GetComponent<TMP_Text>();
#if UNITY_EDITOR
        EditorApplication.hierarchyChanged += Main;
#endif
        Main();
    }
    private void Awake()
    {

        if (_textMesh == null)
            _textMesh = GetComponent<TMP_Text>();
        Main();
    }
    [ContextMenu("Main()")]
    private void Main()
    {
        if (_textMesh != null) _textMesh.text = gameObject.name;

    }
}
