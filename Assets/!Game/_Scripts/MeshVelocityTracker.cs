using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MeshVelocityTracker : MonoBehaviour
{
    private Vector2 _prevPosition = Vector2.zero;
    private List<Vector2> _velocitySamples = new();
    private Animator _animator;
    [SerializeField] private string xAxisParameterName;
    [SerializeField] private string yAxisParameterName;
    [SerializeField] private Character character;
    [SerializeField] private InteractibleObject _interactibleObject;
    private bool _riding;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        if (character != null)
        {
            character.OnAttach += OnAttach;
            character.OnDetach += OnDetach;
        }
    }
    private void OnDisable()
    {
        if (character != null)
        {
            character.OnAttach -= OnAttach;
            character.OnDetach -= OnDetach;
        }
    }
    private void OnAttach(GameObject go)
    {
        _prevPosition = Vector2.zero;
        _riding = true;
        _animator.SetFloat(xAxisParameterName, 0);
        _animator.SetFloat(yAxisParameterName, 0);
    }
    private void OnDetach() { _riding = false; }
    void Update()
    {
        if (_prevPosition != Vector2.zero)
        {
            Vector3 deltaPosition3D = new(transform.position.x - _prevPosition.x, 0, transform.position.z - _prevPosition.y);
            deltaPosition3D = transform.InverseTransformVector(deltaPosition3D);
            Vector2 deltaPosition = new(deltaPosition3D.x, deltaPosition3D.z);
            deltaPosition = Vector2.ClampMagnitude(deltaPosition, 1) / 0.01f;
            deltaPosition = deltaPosition.normalized;
            _velocitySamples.Add(deltaPosition);

            //the number of velocity samples used for the average, scales with the framerate
            while (_velocitySamples.Count > 0.16 / Time.deltaTime) { _velocitySamples.RemoveAt(0); }
            //Debug.Log(velocitySamples.Count);
            Vector2 sum = Vector2.zero;
            for (int i = 0; i < _velocitySamples.Count; i++)
            {
                sum += _velocitySamples[i];
            }
            Vector2 avg = sum / _velocitySamples.Count;
            avg = avg.normalized;
            Debug.DrawRay(transform.position, new(avg.x, 0, avg.y));
            _animator.SetFloat(xAxisParameterName, avg.x);
            _animator.SetFloat(yAxisParameterName, avg.y);
            if (_interactibleObject != null)
            {
                if (avg.x != 0 || avg.y != 0)
                {
                    _interactibleObject.AddEmbargo("Moving");
                }
                else
                {
                    _interactibleObject.RemoveEmbargo("Moving");
                }
            }
        }
        if (!_riding)
        {
            _prevPosition = new(transform.position.x, transform.position.z);
        }

    }
}
