using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MultiStateFork : MonoBehaviour
{
    [System.Serializable]
    private class EventState
    {
        [FormerlySerializedAs("Flag")]
        public string DataFlag;
        [FormerlySerializedAs("GameObject")]
        public UnityEvent Event;
    }
    [SerializeField] private EventState[] _eventStates;
    [SerializeField] private UnityEvent _final;
    [SerializeField] private bool _executeOnAwake;
    public void SaveFlagTrue(int i)
    {
        if (i >= _eventStates.Length || i < 0) return;
        WorldData.WorldFlags.Add(_eventStates[i].DataFlag);
    }
    public void Execute()
    {
        bool foundEnd = false;
        foreach (var state in _eventStates)
        {
            if (foundEnd || WorldData.WorldFlags.Has(state.DataFlag) || string.IsNullOrEmpty(state.DataFlag))
            {
                continue;
            }
            foundEnd = true;
            state.Event.Invoke();
        }
        if (!foundEnd)
            _final.Invoke();
    }
    private void Awake()
    {
        if (_executeOnAwake)
            Execute();
    }
}
