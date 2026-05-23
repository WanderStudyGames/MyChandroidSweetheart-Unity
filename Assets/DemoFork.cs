using UnityEngine;
using UnityEngine.Events;

public class DemoFork : MonoBehaviour
{
    [SerializeField] private UnityEvent _true;
    [SerializeField] private UnityEvent _false;
    [SerializeField] private bool _executeOnAwake = true;
    void Awake()
    {
        if (_executeOnAwake) Execute();
    }
    public void Execute()
    {
        if (Application.version.ToLower().Contains("demo")) { _true.Invoke(); }
        else { _false.Invoke(); }
    }
}
