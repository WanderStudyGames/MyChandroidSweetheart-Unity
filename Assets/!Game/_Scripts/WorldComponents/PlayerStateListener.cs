using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class PlayerStateResponse
{
    [SerializeField] private PlayerState _playerState;
    public PlayerState PlayerState => _playerState;
    [SerializeField] private UnityEvent response;
    public void Subscribe() { PlayerStateManager.OnStateChanged += OnStateChanged; }
    public void Unsubscribe() { PlayerStateManager.OnStateChanged -= OnStateChanged; }
    private void OnStateChanged(PlayerState state) { if (state == _playerState) response.Invoke(); }
}
public class PlayerStateListener : MonoBehaviour
{
    [SerializeField] private List<PlayerStateResponse> _stateChangeResponses;
    public void OnEnable()
    {
        for (int i = _stateChangeResponses.Count - 1; i >= 0; i--)
        {
            if (_stateChangeResponses[i].PlayerState == null) { Debug.LogError($"{gameObject.name}: {this.GetType().Name} null reference exception"); }
            _stateChangeResponses[i].Subscribe();

        }
    }
    public void OnDisable()
    {
        for (int i = _stateChangeResponses.Count - 1; i >= 0; i--)
        {
            _stateChangeResponses[i].Unsubscribe();
        }
    }
}
