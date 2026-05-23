using UnityEngine;
using UnityEngine.Serialization;

public class EventStateMachine : MonoBehaviour
{
    [System.Serializable]
    private class EventState
    {
        [FormerlySerializedAs("Flag")]
        public string DataFlag;
        [FormerlySerializedAs("GameObject")]
        public GameObject EventGroup;
    }
    [SerializeField] private EventState[] _eventStates;
    [SerializeField] private GameObject _final;
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
                if (state.EventGroup != null)
                    state.EventGroup.SetActive(false);
                continue;
            }
            Debug.Log("Event State Machine activating: " + state.DataFlag);
            if (state != null)
                trueOne = state;
        }
        if (_final != null)
            _final.SetActive(trueOne == null);
        if (trueOne != null && trueOne.EventGroup != null)
            trueOne.EventGroup.SetActive(true);
    }
}
