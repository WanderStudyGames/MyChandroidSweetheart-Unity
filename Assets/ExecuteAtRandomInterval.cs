using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteAtRandomInterval : MonoBehaviour
{
    [SerializeField] private Vector2 _secondsRange;
    [SerializeField] private UnityEvent _event;
    private void OnEnable()
    {
        StartCoroutine(Co_Interval());
        IEnumerator Co_Interval()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_secondsRange.x, _secondsRange.y));

                _event.Invoke();
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
