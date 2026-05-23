using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

using VInspector;

public class CharacterBlink : MonoBehaviour
{
    [SerializeField] Material material;
    [FormerlySerializedAs("eyesOpen")]
    [SerializeField] Texture2D eyesOpen1;
    [FormerlySerializedAs("eyesClosed")]
    [SerializeField] Texture2D eyesClosed1;
    [SerializeField] private bool unscaledTime;

    [Foldout("Layer 2")]
    [SerializeField] Texture2D eyesOpen2;
    [SerializeField] Texture2D eyesClosed2;
    [EndFoldout]
    [SerializeField] Light _light;

    [SerializeField] private UnityEvent _onEyesOpen;
    [SerializeField] private UnityEvent _onEyesClosed;

    private Texture2D _overrideTexture;

    public void SetOverrideTexture(Texture2D tex)
    {
        _overrideTexture = tex;
        SetTex(tex);
        SetTex2(null);
    }
    public void ClearOverrideTexture()
    {
        _overrideTexture = null;
        OpenEyes();
    }

    private void SetTex(Texture2D tex)
    {
        if (material == null) return;
        material.SetTexture("_MainTex", tex);
        material.SetTexture("_Tex1", tex);
        material.SetTexture("_Emissive_Tex", tex);
        material.SetTexture("_EmissionMap", tex);
    }
    private void SetTex2(Texture2D tex)
    {
        if (material == null) return;
        material.SetTexture("_Tex2", tex);
    }
    public void OpenEyes()
    {
        if (_overrideTexture != null) return;
        SetTex(eyesOpen1);
        SetTex2(eyesOpen2);
        _onEyesOpen?.Invoke();
        if (_light != null) { _light.enabled = true; }
    }
    public void CloseEyes()
    {
        if (_overrideTexture != null) return;
        SetTex(eyesClosed1);
        SetTex2(eyesClosed2);
        _onEyesClosed?.Invoke();
        if (_light != null) { _light.enabled = false; }
    }
    private void OnEnable()
    {
        StartCoroutine(Co_EyesOpen());
    }
    private void OnDisable()
    {
        OpenEyes();
    }
    private IEnumerator Co_EyesOpen()
    {
        OpenEyes();
        if (unscaledTime)
            yield return new WaitForSecondsRT(Random.Range(3, 5));
        else
            yield return new WaitForSeconds(Random.Range(3, 5));

        StartCoroutine(Co_EyesClosed());
    }

    private IEnumerator Co_EyesClosed()
    {
        CloseEyes();
        if (unscaledTime)
            yield return new WaitForSecondsRT(Random.Range(0.1f, 0.3f));
        else
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        StartCoroutine(Co_EyesOpen());

    }
}
