using UnityEngine;

public class LockTransform : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    private Vector3 _position;
    private void Awake()
    {
        _position = _transform.position;
    }
    void LateUpdate()
    {
        if (_transform != null) transform.position = _transform.position;
        else transform.position = _position;
    }
}
