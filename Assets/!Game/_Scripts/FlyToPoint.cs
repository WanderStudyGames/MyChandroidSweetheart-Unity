using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class FlyToPoint : MonoBehaviour
{
    [SerializeField] private float _startDelay;
    [SerializeField] private float _seconds;
    [SerializeField] private AnimationCurve _exitCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _entryCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private UnityEvent _executeOnStart;
    [SerializeField] private UnityEvent _executeOnFinish;
    public void Fly(Transform target)
    {
        FlyTo(target.position, _seconds);
    }
    public void FlyTo(Vector3 position, float seconds = 0.5f, Action onFinishCallback = null)
    {
        StopAllCoroutines();
        StartCoroutine(Co_FlyAndExecute(position, seconds, onFinishCallback));
    }
    private IEnumerator Co_FlyAndExecute(Vector3 destination, float seconds = 0.5f, Action action = null)
    {
        yield return new WaitForSeconds(_startDelay);
        _executeOnStart.Invoke();
        var time = 0f;
        var start = transform.position;
        var end = transform.position + (Vector3.up * 100f);
        var curve = _exitCurve;
        for (int i = 0; i < 2; i++)
        {
            while (time < seconds)
            {
                transform.position = Vector3.Lerp(start, end, curve.Evaluate(time / seconds));
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = Vector3.Lerp(start, end, 1);

            time = 0f;
            start = destination + (Vector3.up * 100f);
            end = destination;
            curve = _entryCurve;
        }
        action?.Invoke();
        _executeOnFinish.Invoke();
    }
}
