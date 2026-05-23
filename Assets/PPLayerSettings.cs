using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPLayerSettings : MonoBehaviour
{
    [SerializeField] private PostProcessLayer _defaultLayer;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GraphicsSettings _settings;

    private void Awake()
    {
        SetStuff();
    }

    private void OnEnable()
    {
        SetStuff();
        GraphicsSettings.OnApplySettings += SetStuff;

    }
    private void OnDisable()
    {
        GraphicsSettings.OnApplySettings -= SetStuff;
    }
    private void SetStuff()
    {
        _defaultLayer.antialiasingMode = _settings.Data.AntiAliasing ? PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing : PostProcessLayer.Antialiasing.None;
        if (_mainCamera != null)
            _mainCamera.fieldOfView = _settings.Data.FOV;
    }

}
