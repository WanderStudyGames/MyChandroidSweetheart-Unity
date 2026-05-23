using UnityEngine;
using UnityEngine.Events;

public class WorldFlagListener : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    private void OnEnable()
    {
        WorldData.WorldFlags.OnDataModified += Execute;
    }
    private void OnDisable()
    {
        WorldData.WorldFlags.OnDataModified -= Execute;
    }
    private void Execute()
    {
        _event.Invoke();
    }
}
