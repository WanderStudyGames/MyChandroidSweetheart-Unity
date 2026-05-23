using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteUnityEvent : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 0f;
    private enum ExecuteType
    {
        MethodCall,
        Start,
        Awake,
        OnEnable,
        OnDisable
    }
    private void OnDestroy()
    {
        uEvent.RemoveAllListeners();
    }

    [SerializeField] private ExecuteType executeTiming = ExecuteType.MethodCall;
    [SerializeField] private UnityEvent uEvent;
    private void Awake()
    {
        if (executeTiming == ExecuteType.Awake) Execute();
    }
    private void OnEnable()
    {
        if (executeTiming == ExecuteType.OnEnable) Execute();
    }
    private void OnDisable()
    {
        if (executeTiming == ExecuteType.OnDisable) Execute();
    }
    private void Start()
    {
        if (executeTiming == ExecuteType.Start) Execute();
    }
    [ContextMenu("Execute()")]
    public void Execute()
    {
        StopAllCoroutines();
        if (delayInSeconds > 0f)
        {
            StartCoroutine(Co_Execute(delayInSeconds));
        }
        else
        {
            if ((enabled && isActiveAndEnabled)) uEvent.Invoke();

            else if (executeTiming == ExecuteType.OnDisable) uEvent.Invoke();
        }

    }
    private IEnumerator Co_Execute(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enabled && isActiveAndEnabled) uEvent.Invoke();
    }
}
