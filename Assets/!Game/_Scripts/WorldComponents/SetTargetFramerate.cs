using UnityEngine;

public class SetTargetFramerate : MonoBehaviour
{
    public int framerate;
    public void SetFramerate(int i)
    {
        Application.targetFrameRate = i;
    }
    [ContextMenu("SetFramerate()")]
    public void SetFrameRate()
    {
        SetFramerate(framerate);
    }
    private void OnEnable()
    {
        SetFrameRate();
    }
}
