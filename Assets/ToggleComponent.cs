using UnityEngine;
using UnityEngine.Events;

public class ToggleComponent : MonoBehaviour
{
    [SerializeField] private UnityEvent[] _events;
    private int _index = 0;

    public void Execute(int index)
    {
        if (index >= _events.Length || index < 0) return;
        _index = index;
        _events[index].Invoke();
    }
    public void Execute()
    {
        Execute(_index);
        _index++;
        if (_index >= _events.Length) _index = 0;
    }
}
