using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [System.Serializable]
    public class TimedEvent
    {
        public float Delay;
        public UnityEvent Event;
    }
    [Tooltip("If true, the delay of each event is relative to the start of the sequence. If false, the delay of each event is relative to the previous event.")]
    [SerializeField] private bool _absoluteTiming = false;
    [SerializeField] private TimedEvent[] _events;
    public void Execute()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Execute(_events.ToList(), _absoluteTiming));

    }
    public static IEnumerator Co_Execute(List<TimedEvent> events, bool absoluteTiming = false)
    {
        if (!absoluteTiming)
        {
            foreach (var e in events)
            {
                yield return new WaitForSeconds(e.Delay);
                e.Event.Invoke();
            }

        }
        else
        {
            var time = 0f;
            var eventsList = events.ToList();
            while (eventsList.Count > 0)
            {
                time += Time.deltaTime;
                for (int i = eventsList.Count - 1; i >= 0; i--)
                {
                    var e = eventsList[i];
                    if (time >= e.Delay)
                    {
                        e.Event.Invoke();
                        eventsList.RemoveAt(i);
                    }
                }
                yield return null;
            }
        }
    }
}