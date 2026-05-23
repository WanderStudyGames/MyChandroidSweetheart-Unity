using UnityEngine;
using UnityEngine.Events;

public class AnimationEventExecute : MonoBehaviour
{
    public UnityEvent[] events;
    public void AnimEventRaise(int i)
    {
        if (events.Length > 0 && i < events.Length && i >= 0)
        {
            events[i].Invoke();
        }
    }
    public void AnimEventRaise(string s)
    {
        var i = int.Parse(s);
        if (events.Length > 0 && i < events.Length && i >= 0)
        {
            events[i].Invoke();
        }
    }
}
