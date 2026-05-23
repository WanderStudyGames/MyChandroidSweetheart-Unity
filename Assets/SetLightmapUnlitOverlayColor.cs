using UnityEngine;

using VInspector;

public class SetLightmapUnlitOverlayColor : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;
    [Button("Set Color")]
    public void SetColor()
    {
        SetColor(color);
    }
    public static void SetColor(Color color)
    {
        Shader.SetGlobalColor("_LightMapUnlitOverlayColor", color);
    }
}
