using UnityEngine;

public class ReplacePuzzleBackground : MonoBehaviour
{
    [SerializeField] private Cubemap _cubemap;

    [ContextMenu("Set()")]
    private void Set()
    {
        RenderSettings.skybox.SetTexture("_Cubemap", _cubemap);
    }
    private void Awake()
    {
        Set();
    }
}
