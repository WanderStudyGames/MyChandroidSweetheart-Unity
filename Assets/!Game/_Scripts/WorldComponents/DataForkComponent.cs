#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DataForkComponent : MonoBehaviour
{
    [SerializeField] protected string lookup;
    [SerializeField] private GameObject _false;
    [SerializeField] private GameObject _true;
    [SerializeField] private bool _checkOnSave = true;
    private void Awake()
    {
        Check();
    }
    private void OnEnable()
    {
        Check();
    }
    private void Start()
    {

    }

    public void Check()
    {
        if (!enabled) return;
        bool b = Validate();
        if (_true != null) _true.SetActive(b);
        if (_false != null) _false.SetActive(!b);
    }
    public virtual bool Validate()
    {
        return WorldData.WorldFlags.Has(lookup) || WorldData.SceneBools.Has(lookup);
    }

    public virtual void SaveTrue()
    {
        WorldData.WorldFlags.Add(lookup);
        if (_checkOnSave)
            Check();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up, lookup);
#endif
    }
}
