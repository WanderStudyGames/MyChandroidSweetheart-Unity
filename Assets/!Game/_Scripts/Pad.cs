using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Pad : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LerpOnCommand _lerpOnCommand;
    [SerializeField] private SignalOutput _signalOutput;
    [SerializeField] private SFXSource _pressSFXSource;
    [SerializeField] private ParticleSystem _pressParticleSystem;
    [SerializeField] private SFXSource _releaseSFXSource;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private Material _wireOnMaterial;
    [SerializeField] private Material _wireOffMaterial;
    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private UnityEvent _onRelease;
    public void SetWireOnMaterial(Material mat)
    {
        _wireOnMaterial = mat;
    }
    private List<Collider> _colliders = new();
    private void OnTriggerEnter(Collider other)
    {
        if (!layerMask.Contains(other.gameObject.layer)) return;

        if (_colliders.Count == 0)
        {
            _lerpOnCommand.SetDestination(1);
            _signalOutput.On();
            _onPress.Invoke();
            _pressSFXSource.Play();
            _pressParticleSystem?.Play();
            foreach (var renderer in _lineRenderers) { renderer.material = _wireOnMaterial; }
        }
        _colliders.Add(other);
        if (other.TryGetComponent(out PhysicsFriendlyDisabler pfd))
        {
            pfd.BeforeDisable += DisableObject;
        }
    }
    private void DisableObject(PhysicsFriendlyDisabler disabler)
    {
        for (var i = _colliders.Count - 1; i >= 0; i--)
        {
            var col = _colliders[i];
            if (disabler != null && col.gameObject == disabler.gameObject)
            {
                OnTriggerExit(col);
                disabler.BeforeDisable -= DisableObject;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (!layerMask.Contains(other.gameObject.layer)) return;

        _colliders.Remove(other);
        if (_colliders.Count == 0)
        {
            _lerpOnCommand.SetDestination(0);
            _signalOutput.Off();
            _onRelease.Invoke();
            _releaseSFXSource.Play();
            foreach (var renderer in _lineRenderers) { renderer.material = _wireOffMaterial; }
        }
    }
    private void OnEnable()
    {
        _colliders.Clear();
        _lerpOnCommand.SetDestination(0);
        _signalOutput.Off();
        _onRelease.Invoke();
        //_releaseSFXSource.Play();
        foreach (var renderer in _lineRenderers) { renderer.material = _wireOffMaterial; }
    }
}
