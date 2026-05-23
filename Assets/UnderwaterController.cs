using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using VInspector;

public class UnderwaterController : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private LayerMask _waterLayerMask;
    [SerializeField] private UnityEvent _onEnterWater;
    [SerializeField] private UnityEvent _onExitWater;
    [SerializeField] private bool _affectFog = true;
    [SerializeField, ShowIf("_affectFog")] private float _underwaterFogDensity = 0.1f;
    [SerializeField] private Color _underwaterFogColor = Color.blue;
    private bool _isUnderwater = false;
    private float _originalFogDensity;
    private Color _originalFogColor;
    private bool _originalFogEnabled;

    private void Awake()
    {

    }
    private IEnumerator Start()
    {
        yield return null;
        if (_affectFog)
        {
            _originalFogDensity = RenderSettings.fogDensity;
            _originalFogColor = RenderSettings.fogColor;
            _originalFogEnabled = RenderSettings.fog;
        }
    }
    private void OnDestroy()
    {
        SetUnderwaterFog(false);
    }
    private void OnDisable()
    {
        SetUnderwaterFog(false);
    }
    private void SetUnderwaterFog(bool b)
    {
        if (!_affectFog) return;
        if (b)
        {
            RenderSettings.fogDensity = _underwaterFogDensity;
            RenderSettings.fogColor = _underwaterFogColor;
            RenderSettings.fog = true;
        }
        else
        {
            RenderSettings.fogDensity = _originalFogDensity;
            RenderSettings.fogColor = _originalFogColor;
            RenderSettings.fog = _originalFogEnabled;
        }
    }
    private bool IsUnderwater()
    {
        return Physics.Raycast(transform.position + _offset, Vector3.up, 100f, _waterLayerMask);
    }

    void FixedUpdate()
    {
        if (!_isUnderwater && IsUnderwater())
        {
            _isUnderwater = true;
            _onEnterWater.Invoke();
            if (_affectFog) SetUnderwaterFog(true);
        }
        else if (_isUnderwater && !IsUnderwater())
        {
            _isUnderwater = false;
            _onExitWater.Invoke();
            if (_affectFog) SetUnderwaterFog(false);
        }
    }
}
