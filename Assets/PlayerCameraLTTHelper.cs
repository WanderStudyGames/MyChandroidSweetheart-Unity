using UnityEngine;

public class PlayerCameraLTTHelper : MonoBehaviour
{
    [SerializeField] LerpToTransform lerpToTransform;
    public void TeleportToPlayerCameraThenLerp()
    {
        if (PlayerLook.Camera == null) return;
        // lerpToTransform.TeleportThenLerp(PlayerLook.Camera.transform);
    }
}
