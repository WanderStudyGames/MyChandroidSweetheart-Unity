using UnityEngine;

public class SetAmbientColor : MonoBehaviour
{
    public void Set(Color color)
    {
        RenderSettings.ambientSkyColor = color;
    }
}
