using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SprocketTimer : MonoBehaviour, ISprocketPushable
{
    [SerializeField] private float _timeLimit = 5f;
    [SerializeField] private UnityEvent _onTimerStart;
    [SerializeField] private UnityEvent _onTimerEnd;

    public bool Push(Vector3 force)
    {
        StopAllCoroutines();
        _onTimerStart.Invoke();
        StartCoroutine(TimerCoroutine());
        IEnumerator TimerCoroutine()
        {
            yield return new WaitForSeconds(_timeLimit * force.magnitude);
            _onTimerEnd.Invoke();
        }
        return false;
    }
}
