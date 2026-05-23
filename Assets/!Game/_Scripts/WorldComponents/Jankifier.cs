using UnityEngine;

public class Jankifier : MonoBehaviour
{
    public bool jankmode;
    private void Start()
    {
        if (jankmode)
        {
            Application.targetFrameRate = 25;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }
}