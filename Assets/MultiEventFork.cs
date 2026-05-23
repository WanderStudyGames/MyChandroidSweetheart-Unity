using UnityEngine;
using UnityEngine.Events;

public class MultiEventFork : MonoBehaviour
{
    [System.Serializable]
    private class EventState
    {
        public string DataFlag;
        public UnityEvent Event;
    }
    [SerializeField] private EventState[] _eventStates;
    [SerializeField] private UnityEvent _final;
    [SerializeField] private bool _executeOnAwake = true;
    public void SaveFlagTrue(int i)
    {
        if (i >= _eventStates.Length || i < 0) return;
        WorldData.WorldFlags.Add(_eventStates[i].DataFlag);
    }
    private void Awake()
    {
        if (_executeOnAwake)
            Check();
    }
    public void Check()
    {
        EventState trueOne = null;
        foreach (var state in _eventStates)
        {
            if (trueOne != null || WorldData.WorldFlags.Has(state.DataFlag) || WorldData.SceneBools.Has(state.DataFlag) || string.IsNullOrEmpty(state.DataFlag))
            {
                continue;
            }
            Debug.Log("Event State Machine activating: " + state.DataFlag);
            if (state != null)
                trueOne = state;
        }
        if (trueOne == null)
            _final.Invoke();
        else
            trueOne.Event.Invoke();
    }
}
