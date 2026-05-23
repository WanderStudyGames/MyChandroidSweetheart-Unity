using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionEvent : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private UnityEvent _onRelease;
    private void OnEnable()
    {
        _actionReference.action.Link(OnAction);
        _actionReference.action.Enable();
    }
    private void OnDisable()
    {
        _actionReference.action.UnLink(OnAction);
    }
    private void OnAction(InputAction.CallbackContext ctx)
    {
        if (!enabled) return;
        GlobalPlayerInput.SetControlScheme(ctx.GetScheme());
        if (ctx.action.WasPressedThisFrame())
        {
            _onPress.Invoke();
        }
        else if (ctx.canceled)
        {
            _onRelease.Invoke();
        }
    }
}
