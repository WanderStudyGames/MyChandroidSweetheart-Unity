using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AutoHideVirtualMouse : MonoBehaviour
{
    [SerializeField] private InputActionReference _stickAction;
    [SerializeField] private InputActionReference _mousePointAction;
    [SerializeField] private InputActionReference _clickAction;
    [SerializeField] private InputActionReference _scrollAction;
    [SerializeField] private Graphic _image;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private float _scrollMultiplier = 20f;

    private Mouse _virtualMouse;
    public Vector2 VirtualMousePosition { get; private set; }
    public bool Active => _virtualMouse != null && _virtualMouse.added;


    private RectTransform _cursorTransform;

    private Vector2 _delta;
    private void Awake()
    {
        _cursorTransform = GetComponent<RectTransform>();
        _virtualMouse = InputSystem.GetDevice<Mouse>("VirtualMouse");
    }
    private void OnEnable()
    {
        StartCoroutine(Co_OnEnable());
        IEnumerator Co_OnEnable()
        {
            yield return null;
            if (_virtualMouse == null)
            {
                _virtualMouse = InputSystem.AddDevice<Mouse>("VirtualMouse");
            }

            SetCursorEnabled(false);

            _stickAction.action.Enable();
            _mousePointAction.action.Enable();
            _clickAction.action.Enable();
            _scrollAction.action.Enable();

            _stickAction.action.Link(OnStickMove);
            _mousePointAction.action.performed += OnMouseMove;
            _clickAction.action.performed += OnClick;

            InputSystem.onAfterUpdate += OnAfterUpdate;
        }
    }

    private void OnDisable()
    {
        SetCursorEnabled(false);

        InputSystem.onAfterUpdate -= OnAfterUpdate;

        _stickAction.action.UnLink(OnStickMove);
        _mousePointAction.action.performed -= OnMouseMove;
        _clickAction.action.performed -= OnClick;
    }
    private void SetCursorEnabled(bool enabled)
    {
        if (Cursor.lockState == CursorLockMode.None)
            Cursor.visible = !enabled;
        _image.enabled = enabled;

        if (_virtualMouse == null) return;
        if (enabled)
        {

            if (!_virtualMouse.added) InputSystem.AddDevice(_virtualMouse);

            //move virtual mouse to screen center instead of having it start at (0,0)
            rawVMousePosition = new Vector2(_canvasTransform.rect.width / 2, _canvasTransform.rect.height / 2);
        }
        else
        {
            if (Active)
                RemoveVirtualMouse();
        }
    }



    private Vector2 rawVMousePosition;
    private void OnAfterUpdate()
    {
        if (!_virtualMouse.added) return;


        var newRawPosition = (rawVMousePosition + _delta * UIManager.VirtualMouseSpeed * Time.unscaledDeltaTime);

        newRawPosition.x = Mathf.Clamp(newRawPosition.x, 0, _canvasTransform.rect.width);
        newRawPosition.y = Mathf.Clamp(newRawPosition.y, 0, _canvasTransform.rect.height);

        rawVMousePosition = newRawPosition;

        var newPositionTranslated = new Vector2(
            newRawPosition.x * (Screen.width / _canvasTransform.rect.width),
            newRawPosition.y * (Screen.height / _canvasTransform.rect.height)
            );

        //move virtual mouse screen position

        InputState.Change(_virtualMouse.delta, newPositionTranslated - _virtualMouse.position.ReadValue());
        InputState.Change(_virtualMouse.position, newPositionTranslated);
        VirtualMousePosition = newPositionTranslated;
        //move cursor transform accordingly
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasTransform, newPositionTranslated, null, out Vector3 localPoint);
        _cursorTransform.position = localPoint;

        InputState.Change(_virtualMouse.scroll, new Vector2(0f, _scrollAction.action.ReadValue<float>() * _scrollMultiplier * Time.unscaledDeltaTime * 100f));
        //InputSystem.QueueDeltaStateEvent<Vector2>(_virtualMouse.scroll, new Vector2(0f, _scrollAction.action.ReadValue<float>() * _scrollMultiplier));
    }

    private void OnStickMove(InputAction.CallbackContext ctx)
    {
        GlobalPlayerInput.SetControlScheme(ctx.GetScheme());
        if (!_image.enabled)
        {
            SetCursorEnabled(true);
        }
        if (!_virtualMouse.added) return;
        _delta = ctx.ReadValue<Vector2>();
    }
    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (!_virtualMouse.added) return;
        _virtualMouse.CopyState(out MouseState mouseState);
        mouseState.WithButton(MouseButton.Left, ctx.ReadValueAsButton());
        InputState.Change(_virtualMouse, mouseState);
    }
    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        if (_image.enabled && ctx.control.device.native && ctx.control.device is Mouse)
        {
            StartCoroutine(Co_EndOfFrame());
        }
        //using local coroutine because removing the device before end of frame causes errors
        IEnumerator Co_EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            SetCursorEnabled(false);
        }
    }
    private void RemoveVirtualMouse()
    {
        bool enableStickAction = _stickAction.action.enabled;
        bool enableMousePointAction = _mousePointAction.action.enabled;
        bool enableClickAction = _clickAction.action.enabled;
        bool enableScrollAction = _scrollAction.action.enabled;

        _stickAction.action.Disable();
        _mousePointAction.action.Disable();
        _clickAction.action.Disable();
        _scrollAction.action.Disable();

        InputSystem.RemoveDevice(_virtualMouse);

        if (enableStickAction) _stickAction.action.Enable();
        if (enableMousePointAction) _mousePointAction.action.Enable();
        if (enableClickAction) _clickAction.action.Enable();
        if (enableScrollAction) _scrollAction.action.Enable();
    }

}