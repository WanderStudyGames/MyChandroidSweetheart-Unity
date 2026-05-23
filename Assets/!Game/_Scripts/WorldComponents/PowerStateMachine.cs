using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum PowerState
{
    Inactive,
    Active,
    Powered
}
public class PowerStateMachine : MonoBehaviour
{
    [SerializeField] private PowerState _defaultState;
    [SerializeField] private GameObject _inactiveGroup;
    [SerializeField] private GameObject _activeGroup;
    [SerializeField] private GameObject _poweredGroup;
    [SerializeField] private bool _changeStateImmediatelyOnPowered;
    [SerializeField] private UnityEvent _onPowered;
    private PowerStateMachineController _controller;
    public void SetState(PowerState powerState)
    {
        if (_inactiveGroup != null && powerState != PowerState.Inactive)
            _inactiveGroup.SetActive(false);
        if (_activeGroup != null && powerState != PowerState.Active)
            _activeGroup.SetActive(false);
        if (_poweredGroup != null && powerState != PowerState.Powered)
            _poweredGroup.SetActive(false);
        switch (powerState)
        {
            case PowerState.Inactive:
                if (_inactiveGroup != null)
                    _inactiveGroup.SetActive(true);
                break;
            case PowerState.Active:
                if (_activeGroup != null)
                    _activeGroup?.SetActive(true);
                break;
            case PowerState.Powered:
                if (_poweredGroup != null)
                    _poweredGroup?.SetActive(true);
                break;
        }
    }
    private void Awake()
    {
        _controller = GetComponentInParent<PowerStateMachineController>();
        //if (_controller == null) Debug.LogError($"{name}: {nameof(PowerStateMachine)} component must be a direct child of {nameof(PowerStateMachineController)}!", gameObject);
        SetState(_defaultState);
    }
    private void OnEnable()
    {
        if (_controller != null)
            _controller.OnPowered += OnPowered;
    }
    private void OnDisable()
    {
        if (_controller != null)
            _controller.OnPowered -= OnPowered;
    }
    private void OnPowered()
    {
        if (_changeStateImmediatelyOnPowered)
            SetState(PowerState.Powered);
        _onPowered.Invoke();
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            EditorApplication.delayCall += () => { SetState(_defaultState); };
    }
#endif
}
