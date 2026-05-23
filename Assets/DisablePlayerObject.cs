using UnityEngine;

public class DisablePlayerObject : MonoBehaviour
{
    public void DisablePlayer()
    {
        PlayerManager.SetPlayerEnabled(false);
    }
    public void EnablePlayer()
    {
        PlayerManager.SetPlayerEnabled(true);
    }
}
