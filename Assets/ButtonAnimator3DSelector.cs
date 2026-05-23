using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonAnimator3DSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private InputActionReference _clickAction;
    [SerializeField] private AutoHideVirtualMouse _autoHideVirtualMouse;
    [SerializeField] private LayerMask _layers;
    private ButtonAnimator _buttonAnimator;

    private void OnEnable()
    {
        _clickAction.action.Link(OnClick);
    }
    private void OnDisable()
    {
        _clickAction.action.UnLink(OnClick);
    }
    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame() && _buttonAnimator != null)
        {
            _buttonAnimator.OnPointerDown(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var position = (_autoHideVirtualMouse.Active) ? _autoHideVirtualMouse.VirtualMousePosition : Mouse.current.position.ReadValue();
        var ray = _camera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layers, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.gameObject.TryGetComponent(out ButtonAnimator buttonAnimator))
            {
                if (buttonAnimator != _buttonAnimator)
                {
                    if (_buttonAnimator != null) { _buttonAnimator.OnPointerExit(null); }
                    _buttonAnimator = buttonAnimator;
                    buttonAnimator.OnPointerEnter(null);
                }
            }
            //no component
            else if (_buttonAnimator != null)
            {
                _buttonAnimator.OnPointerExit(null);
                _buttonAnimator = null;
            }
        }
        //no collider
        else if (_buttonAnimator != null)
        {
            _buttonAnimator.OnPointerExit(null);
            _buttonAnimator = null;
        }
    }
}
