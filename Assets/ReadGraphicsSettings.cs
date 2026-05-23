using UnityEngine;
using UnityEngine.UI;

public class ReadGraphicsSettings : MonoBehaviour
{
    [SerializeField] private CarouselSelector _aspectRatio;
    [SerializeField] private CarouselSelector _fullscreenMode;
    [SerializeField] private CarouselSelector _quality;
    [SerializeField] private GraphicsSettings _graphicsSettings;
    [SerializeField] private Toggle _ambientOcclusion;
    [SerializeField] private Toggle _motionBlur;
    [SerializeField] private Toggle _chromaticAberration;
    [SerializeField] private Toggle _depthOfField;
    [SerializeField] private Toggle _bloom;
    [SerializeField] private Toggle _colorGrading;
    [SerializeField] private Toggle _antiAliasing;
    [SerializeField] private Slider _brightness;
    [SerializeField] private Slider _resolutionMultiplier;
    [SerializeField] private Slider _fov;
    private void Awake()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        if (_aspectRatio != null) _aspectRatio.SetValue((int)_graphicsSettings.Data.AspectRatio);
        if (_fullscreenMode != null) _fullscreenMode.SetValue((int)_graphicsSettings.Data.FullScreen);
        _ambientOcclusion.isOn = _graphicsSettings.Data.AmbientOcclusion;
        _motionBlur.isOn = _graphicsSettings.Data.MotionBlur;
        _chromaticAberration.isOn = _graphicsSettings.Data.ChromaticAberration;
        _depthOfField.isOn = _graphicsSettings.Data.DepthOfField;
        _bloom.isOn = _graphicsSettings.Data.Bloom;
        _colorGrading.isOn = _graphicsSettings.Data.ColorGrading;
        _antiAliasing.isOn = _graphicsSettings.Data.AntiAliasing;
        _brightness.value = _graphicsSettings.Data.Brightness;
        _brightness.onValueChanged.Invoke(_brightness.value);
        _resolutionMultiplier.value = _graphicsSettings.Data.ResolutionMultiplier;
        _resolutionMultiplier.onValueChanged.Invoke(_resolutionMultiplier.value);
        _quality.SetValue(_graphicsSettings.Data.Quality);
        _fov.value = _graphicsSettings.Data.FOV;
        _fov.onValueChanged.Invoke(_fov.value);
    }
}
