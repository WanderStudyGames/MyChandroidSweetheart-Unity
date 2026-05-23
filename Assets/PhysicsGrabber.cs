using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PhysicsGrabber : MonoBehaviour
{
    [SerializeField] private LayerMask _rayToPlaneLayers;
    [SerializeField] private AutoHideVirtualMouse _autoHideVirtualMouse;
    [SerializeField] private InputActionReference _grabAction;

    [SerializeField] private LayerMask _physicsLayers;
    [SerializeField] private Collider _grabberCollider;
    [SerializeField] private Joint _joint;
    [SerializeField] private Camera _camera;

    [SerializeField] private UnityEvent _onGrab;
    [SerializeField] private UnityEvent _onRelease;

    private Collider _grabbedCollider;
    private void Awake()
    {
        _joint.connectedBody = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_physicsLayers.Contains(other.gameObject.layer) && other.attachedRigidbody != null)
        {
            _grabbedCollider = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_grabbedCollider == other)
        {
            _grabbedCollider = null;
        }
    }
    private void OnEnable()
    {
        _grabAction.action.Link(OnGrab);
    }
    private void OnDisable()
    {
        _grabAction.action.UnLink(OnGrab);
    }
    private void OnGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            _onGrab.Invoke();
            if (_grabbedCollider == null) return;
            _joint.connectedBody = _grabbedCollider.attachedRigidbody;
            _joint.connectedAnchor = _grabbedCollider.attachedRigidbody.transform.InverseTransformPoint(_grabbedCollider.ClosestPoint(_grabberCollider.transform.position));

        }
        if (ctx.action.WasReleasedThisFrame())
        {
            _onRelease.Invoke();
            _joint.connectedBody = null;

        }
    }
    void Update()
    {
        var position = (_autoHideVirtualMouse.Active) ? _autoHideVirtualMouse.VirtualMousePosition : Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _rayToPlaneLayers, QueryTriggerInteraction.Collide))
        {
            transform.position = hit.point;
        }
    }


}
