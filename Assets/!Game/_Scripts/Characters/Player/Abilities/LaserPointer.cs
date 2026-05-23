using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLook))]
[RequireComponent(typeof(PlayerInput))]
public class LaserPointer : PlayerComponent
{
    private PlayerInput _playerInput;
    private LaserPointerProfile _profile;
    private GameObject _pointerTarget;
    private Camera _camera;
    public override void SetComponentProfile(ComponentProfile profile)
    {
        _profile = profile as LaserPointerProfile;
    }
    private void Awake()
    {
        _camera = GetComponent<PlayerLook>().GetCamera();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions.Link("Laser", OnLaser);
    }
    private void OnDisable()
    {
        _playerInput.actions.UnLink("Laser", OnLaser);
    }
    public static event Action OnCommand;
    private void OnLaser(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            //start laser command
            _pointerTarget = new GameObject();
            _pointerTarget.tag = "Laser";
            StopAllCoroutines();
            StartCoroutine(Co_UpdateTargetPosition());
            CompanionData.Command(_pointerTarget.transform);
            OnCommand?.Invoke();
        }
        if (ctx.canceled)
        {
            //end laser command
            Cancel();
        }
    }
    private void Cancel()
    {
        if (_pointerTarget != null) Destroy(_pointerTarget);
        _pointerTarget = null;
    }
    public IEnumerator Co_UpdateTargetPosition()
    {
        while (_pointerTarget != null)
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, 100f, _profile.LayerMask, QueryTriggerInteraction.Ignore))
            {
                _pointerTarget.transform.position = hit.point;
            }
            else
            {
                Cancel();
            }
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        if (_pointerTarget != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(_pointerTarget.transform.position, Vector3.one);
        }
    }
}
