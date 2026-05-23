using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnLookedAt : MonoBehaviour
{
    [SerializeField] private UnityEvent _onLookAt;
    public void LookAt()
    {
        _onLookAt.Invoke();
    }
}
