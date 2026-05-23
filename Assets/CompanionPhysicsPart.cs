using UnityEngine;
using UnityEngine.Events;

public class CompanionPhysicsPart : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    [SerializeField] private GameObject _target;
    [SerializeField] private UnityEvent _onAttach;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _target)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _rigidbody.transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            _onAttach.Invoke();
        }
    }
}
