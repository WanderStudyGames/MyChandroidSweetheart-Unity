using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerLookSelector))]
public class PlayerStateManager : MonoBehaviour
{
    [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
    [field: SerializeField] public PlayerLookSelector PlayerLookSelector { get; private set; }
    public static PlayerState PreviousState { get; private set; }
    private static PlayerState _state;
    private static PlayerStateManager instance;
    public static PlayerState State => (instance != null) ? _state : null;
    public static event Action<PlayerState> OnStateChanged;

    private void Awake()
    {
        instance = this;
        PreviousState = _state;
        if (_state == null)
            _state = PlayerStates.Default;
    }
    private void Start()
    {
        if (_state == null)
            _state = PlayerStates.Default;
        _state.Enable(this);
    }
    private void OnDisable()
    {
        _state.Disable(null);
        PreviousState = _state;
        _state = PlayerStates.Default;
        _state.Enable(this);
        OnStateChanged?.Invoke(_state);
    }
    public static void SwitchState(PlayerState state)
    {
        if (state == _state) return;

        _state.Disable(state);

        PreviousState = _state;

        _state = state;

        _state.Enable(instance);

        OnStateChanged?.Invoke(_state);
    }
    private void Update()
    {
        if (State != null)
            State.OnUpdate();//
    }
}

