using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnAndroid : MonoBehaviour
{
    [SerializeField] private UnityEvent _execute;
    private void Awake()
    {
#if PLATFORM_ANDROID
        _execute.Invoke();
#endif
    }
}
