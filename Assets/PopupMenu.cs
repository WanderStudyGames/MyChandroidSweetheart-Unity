using UnityEngine;

public class PopupMenu : MonoBehaviour
{
    private PlayerState _state;
    private void OnEnable()
    {
        if (_state == null)
            _state = PlayerStateManager.State;
        PlayerStateManager.SwitchState(PlayerStates.Menu);
    }
    private void OnDisable()
    {
        PlayerStateManager.SwitchState(_state);
        _state = null;
    }
}
