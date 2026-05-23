using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChemicalTube : MonoBehaviour
{
    [Dependency][SerializeField] private SFXSource _batteryExitSFXSource;
    [Dependency][SerializeField] private GameEvent _onChemicalsMixGE;
    [Dependency][SerializeField] private BatterySceneLoader _batterySceneLoader;
    [Dependency][SerializeField] private SceneBoolFork _sceneBoolFork;
    [Dependency][SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Material _chemicalTubeMaterial;
    [SerializeField] private Material _chemicalTubeStillMaterial;
    [SerializeField] private float _transitionDurationInSeconds;
    [SerializeField] private float _waitForLoadDuration;
    [SerializeField] private Color _vertexColor1;
    [SerializeField] private Color _vertexColor2;
    [SerializeField] private UnityEvent _onMixChemicals;
    private int _overrideColorID = Shader.PropertyToID("_OverrideColor");
    private void Awake()
    {
        PauseMenu.DisablePause = false;
    }
    void Start()
    {
        UpdateColor();
    }
    private void OnValidate()
    {
        UpdateColor();
    }
    private void UpdateColor()
    {
        _chemicalTubeMaterial.SetColor("_VertexColor1", _vertexColor1);
        _chemicalTubeStillMaterial.SetColor("_VertexColor1", _vertexColor1);
        _chemicalTubeMaterial.SetColor("_VertexColor2", _vertexColor2);
        _chemicalTubeStillMaterial.SetColor("_VertexColor2", _vertexColor2);

    }
    public void SetOverrideAlpha(float alpha, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ExtensionMethods.Co_FadeFloat(duration, new(_chemicalTubeMaterial.GetColor(_overrideColorID).a, alpha), fl =>
        {
            Color c = _chemicalTubeMaterial.GetColor(_overrideColorID);
            c.a = fl;
            _chemicalTubeMaterial.SetColor(_overrideColorID, c);
        }));
    }
    public void MixChemicals()
    {
        if (WorldData.SceneBools.Has(SceneManager.GetActiveScene().name)) return;
        SetOverrideAlphaImmediate(0);
        SetOverrideAlpha(1, _transitionDurationInSeconds);
        _onChemicalsMixGE.Raise();
        _sceneBoolFork.SaveTrue();
        _onMixChemicals.Invoke();
        PauseMenu.DisablePause = true;
    }
    public void LoadScene()
    {
        PauseMenu.DisablePause = false;
        _batteryExitSFXSource.Play();
        _batterySceneLoader.LoadScene();
        _particleSystem.Play();
    }
    public void SetOverrideAlphaImmediate(float alpha)
    {
        Color c = _chemicalTubeMaterial.GetColor(_overrideColorID);
        c.a = alpha;
        _chemicalTubeMaterial.SetColor(_overrideColorID, c);
    }
}
