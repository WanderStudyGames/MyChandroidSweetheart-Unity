using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "GraphicsSettings", menuName = "ScriptableObjects/Graphics/GraphicsSettings")]
public class GraphicsSettings : ScriptableObject
{
    [field: SerializeField] public GraphicsSettingsData Data { get; private set; }
    [SerializeField] private PostProcessProfile _defaultPPProfile;
    public static event Action OnApplySettings;
    private static Vector2 _windowResolution;
    private void OnEnable()
    {
        SaveSystem.OnSaveStatic += Save;
        PlayMode.OnEnterPlayMode += Init;
        Init();
    }
    private void Init()
    {

        _windowResolution = new(Screen.width, Screen.height);
        Data = ES3.Load("graphicsSettings", "settings.es3", new GraphicsSettingsData());
        ApplySettings();
        ApplyWindowSettings();
    }
    private void Save()
    {
        ES3.Save("graphicsSettings", Data, "settings.es3");
    }
    public class GraphicsSettingsData
    {
        public bool MotionBlur = false;
        public bool DepthOfField = true;
        public bool ChromaticAberration = false;
        public bool ColorGrading = true;
        public bool Bloom = true;
        public bool AmbientOcclusion = true;
        public bool AntiAliasing = true;
        public FullScreenMode FullScreen = FullScreenMode.FullScreenWindow;
        public AspectRatio AspectRatio = AspectRatio.Aspect16by9;
        public float Brightness = 0f;
        public float ResolutionMultiplier = 1f;
        public int Quality = 5;
        public float FOV = 70;
    }
    public void ApplySettings()
    {
#if PLATFORM_ANDROID
        Data.MotionBlur = false;
        Data.AmbientOcclusion = false;
        Data.DepthOfField = false;
        Data.ChromaticAberration = false;
        Data.Bloom = false;
        Data.AntiAliasing = false;
        Data.Quality = 5;
#endif
        if (_defaultPPProfile.TryGetSettings(out MotionBlur motionBlur))
        {
            motionBlur.active = Data.MotionBlur;
        }
        if (_defaultPPProfile.TryGetSettings(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.active = Data.ChromaticAberration;
        }
        if (_defaultPPProfile.TryGetSettings(out AmbientOcclusion ambientOcclusion))
        {
            ambientOcclusion.active = Data.AmbientOcclusion;
        }
        if (_defaultPPProfile.TryGetSettings(out DepthOfField depthOfField))
        {
            depthOfField.active = Data.DepthOfField;
        }
        if (_defaultPPProfile.TryGetSettings(out ColorGrading colorGrading))
        {
            colorGrading.active = Data.ColorGrading;
            colorGrading.brightness.value = Data.Brightness;
        }
        if (_defaultPPProfile.TryGetSettings(out Bloom bloom))
        {
            bloom.active = Data.Bloom;
        }

        QualitySettings.SetQualityLevel(Data.Quality);

        OnApplySettings?.Invoke();
    }
    public void ApplyWindowSettings()
    {
#if PLATFORM_ANDROID
        Data.AspectRatio = AspectRatio.Off;
#endif
        var multiplier = 1f;
        switch (Data.AspectRatio)
        {
            case AspectRatio.Off:
                break;
            case AspectRatio.Aspect16by9:
                multiplier = 16f / 9f;
                break;
            case AspectRatio.Aspect16by10:
                multiplier = 16f / 10f;
                break;
            case AspectRatio.Aspect4by3:
                multiplier = 4f / 3f;
                break;
            case AspectRatio.Aspect5by4:
                multiplier = 5f / 4f;
                break;
        }
        if (Data.AspectRatio == AspectRatio.Off)
        {
            Screen.SetResolution(
                (int)(_windowResolution.x * Data.ResolutionMultiplier),
                (int)(_windowResolution.y * Data.ResolutionMultiplier),
                Data.FullScreen);
            return;
        }
        var height = Data.ResolutionMultiplier * _windowResolution.y;
        Screen.SetResolution((int)(height * multiplier), (int)height, Data.FullScreen);
    }
    public void SetAmbientOcclusion(bool b) { Data.AmbientOcclusion = b; ApplySettings(); }
    public void SetMotionBlur(bool b) { Data.MotionBlur = b; ApplySettings(); }
    public void SetDepthOfField(bool b) { Data.DepthOfField = b; ApplySettings(); }
    public void SetChromaticAberration(bool b) { Data.ChromaticAberration = b; ApplySettings(); }
    public void SetColorGrading(bool b) { Data.ColorGrading = b; ApplySettings(); }
    public void SetBloom(bool b) { Data.Bloom = b; ApplySettings(); }
    public void SetAntiAliasing(bool b) { Data.AntiAliasing = b; ApplySettings(); }
    public void SetQuality(int quality) { Data.Quality = Mathf.Clamp(quality, 0, 5); ApplySettings(); }
    public void SetFullScreen(int mode)
    {
        mode = Mathf.Clamp(mode, 0, 3);
        Data.FullScreen = (FullScreenMode)mode;
        ApplyWindowSettings();
    }
    public enum AspectRatio
    {
        Off,
        Aspect16by9,
        Aspect16by10,
        Aspect4by3,
        Aspect5by4
    }
    public void SetAspectRatio(int ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 4);
        Data.AspectRatio = (AspectRatio)ratio;
        ApplyWindowSettings();
    }
    public void SetBrightness(float brightness)
    {
        Data.Brightness = Mathf.Clamp(brightness, -100, 100);
        ApplySettings();
    }
    public void SetResolutionMultiplier(float resolutionMultiplier)
    {
        Data.ResolutionMultiplier = Mathf.Clamp(resolutionMultiplier, 0.2f, 1f);
        ApplyWindowSettings();
    }
    public void SetFOV(float fov)
    {
        Data.FOV = Mathf.Clamp(fov, 70, 120);
        ApplySettings();
    }
}