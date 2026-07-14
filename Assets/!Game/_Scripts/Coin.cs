using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Coin : MonoBehaviour
{
    [field: SerializeField] public Clothing _clothing { get; private set; }
    [field: SerializeField] public Wardrobe _wardrobe { get; private set; }
    private void Awake()
    {
        gameObject.SetActive(!_wardrobe.Clothings.Has(_clothing));
    }

    public void Collect()
    {
        _wardrobe.AddClothing(_clothing);
        FabricIndicator.Show(_clothing.Name + " Acquired");
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = Color.blue;
        Handles.Label(transform.position, new GUIContent(_clothing != null ? _clothing.Name : "Null"), style);
    }
#endif
}
