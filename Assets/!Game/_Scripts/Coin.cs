using System.Linq;

using UnityEngine;

using VInspector;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Coin : MonoBehaviour
{
    [field: SerializeField] public string Id { get; private set; }

#if UNITY_EDITOR
    [Button("ResetID()")]
    private void ResetID()
    {
        Id = $"{gameObject.scene.name}({gameObject.name},{transform.position})";
    }
    private void OnValidate()
    {
        if (FindObjectsOfType<Coin>().Any(coin => (coin.Id == Id && coin != this)))
        {
            ResetID();
            //Debug.Log("duplicate");
            EditorUtility.SetDirty(this);
        }
        if (string.IsNullOrEmpty(Id) && PrefabUtility.IsPartOfPrefabInstance(this))
        {
            ResetID();
            Debug.Log("empty");
            EditorUtility.SetDirty(this);
        }
    }
#endif
    private void Awake()
    {
        gameObject.SetActive(!WorldData.CollectedCoins.Has(Id));
    }

    public void Collect()
    {
        WorldData.CollectedCoins.Add(Id);
        FabricIndicator.Show(Id);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = Color.blue;
        Handles.Label(transform.position, new GUIContent(Id), style);
    }
#endif
}
