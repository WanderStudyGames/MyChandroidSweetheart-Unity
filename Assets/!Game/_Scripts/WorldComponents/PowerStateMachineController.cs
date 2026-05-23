using System;
using UnityEditor;
using UnityEngine;

public class PowerStateMachineController : MonoBehaviour
{
    [SerializeField] private string _powerStateString;
    [Dependency][SerializeField] private BatterySceneLoader _puzzleScene;
    [SerializeField] private bool _debugPoweredOnStart;

    private PowerStateMachine[] _powerStateMachines;

    public event Action OnPowered;
    public void PowerOn() { WorldData.WorldFlags.Add(_powerStateString); OnPowered?.Invoke(); }
    public void SetState(PowerState powerState)
    {
        foreach (var psm in _powerStateMachines) { psm.SetState(powerState); }
    }
    private void Awake()
    {
        _powerStateMachines = gameObject.GetComponentsInChildren<PowerStateMachine>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (_debugPoweredOnStart) WorldData.WorldFlags.Add(_powerStateString);
#endif
        bool power = WorldData.WorldFlags.Has(_powerStateString);
        bool puzzle = WorldData.SceneBools.Has(_puzzleScene.SceneName);

        if (power) { SetState(PowerState.Powered); }
        else
        {
            if (puzzle) { SetState(PowerState.Active); }
            else SetState(PowerState.Inactive);
        }
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up, _powerStateString);
        if (_puzzleScene != null)
            Handles.Label(transform.position + (Vector3.up * 1.1f), _puzzleScene.SceneName);
#endif
    }
}
