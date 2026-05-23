using UnityEngine;
using UnityEngine.Events;

public class CompanionWithPlayerFork : MonoBehaviour
{
    [SerializeField] private bool _checkOnAwake = true;
    [SerializeField] private bool _setActive = true;

    [SerializeField] private UnityEvent _true;
    [SerializeField] private UnityEvent _false;
    void Awake()
    {
        if (_checkOnAwake) Check();
    }
    public void Check()
    {
        if (_setActive) gameObject.SetActive(CompanionManager.CompanionWithPlayer);
        if (CompanionManager.CompanionWithPlayer) _true.Invoke();
        else _false.Invoke();
    }
}
