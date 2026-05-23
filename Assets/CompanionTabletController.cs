using UnityEngine;
using UnityEngine.Events;

public class CompanionTabletController : MonoBehaviour
{
    [SerializeField] private UnityEvent _onGrabPlayer;
    [SerializeField] private UnityEvent _onReleasePlayer;
    public void GrabPlayer()
    {
        TabletPlayerState.CompanionHeld = true;
        PlayerStateManager.SwitchState(PlayerStates.Tablet);
        _onGrabPlayer.Invoke();
    }
    public void ReleasePlayer()
    {
        if (PlayerStateManager.State != PlayerStates.Tablet || !TabletPlayerState.CompanionHeld) return;

        PlayerStateManager.SwitchState(PlayerStateManager.PreviousState);
        TabletPlayerState.CompanionHeld = false;
        _onReleasePlayer.Invoke();
    }
}
