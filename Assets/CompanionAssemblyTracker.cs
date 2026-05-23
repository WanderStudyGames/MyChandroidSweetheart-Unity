using UnityEngine;
using UnityEngine.Events;

public class CompanionAssemblyTracker : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private UnityEvent _onSuccess;
    public void CheckForSuccess()
    {
        bool success = true;
        foreach (var rigidbody in _rigidbodies)
        {
            if (!rigidbody.isKinematic) success = false;
        }
        if (success)
        {
            _onSuccess.Invoke();
        }
    }
}
