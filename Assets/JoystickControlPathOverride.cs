using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class JoystickControlPathOverride : MonoBehaviour
{
    [SerializeField] private JoystickControlPathOverrides _overrides;
    [SerializeField] private KeyboardIcons _icons;
    [SerializeField, Min(0)] private int _overrideIndex;
    [SerializeField, ReadOnly] private string _overrideKey;


    [SerializeField, EndReadOnlyGroup] private TMP_Text _buttonLabel;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;

    public static event Action OnJoystickOverrideSet;
    private void OnValidate()
    {
        if (_overrideIndex > _overrides.KeyCount) _overrideIndex = _overrides.KeyCount - 1;
        _overrideKey = _overrides.GetKey(_overrideIndex);
        ResetLabel();
    }
    public void SetValue(string controlPath)
    {
        _overrides.SetOverrideValue(_overrideIndex, controlPath);
    }
    private static InputActionRebindingExtensions.RebindingOperation _operation;
    private void OnEnable()
    {
        if (_icons.TryGetSprite(_overrides.GetKey(_overrideIndex), "", out BindingImage bindingImage))
        {
            _icon.sprite = bindingImage.sprite;
        }
        ResetLabel();
    }
    private void OnDisable()
    {
        if (_operation != null && _operation.started) _operation.Cancel();
    }
    public void ResetOverride()
    {
        _overrides.SetOverrideValue(_overrideIndex, "");
        ResetLabel();
    }
    public void Listen()
    {
        _button.interactable = false;
        _buttonLabel.text = "Awaiting input...";

        _operation = new InputActionRebindingExtensions.RebindingOperation()
            .WithCancelingThrough("<Keyboard>/escape")
            .WithTimeout(5)
            .WithBindingGroup("Joystick")
            .WithControlsHavingToMatchPath("<Joystick>")
            .WithControlsExcluding("<Mouse>")
            .WithExpectedControlType<ButtonControl>()
            .OnMatchWaitForAnother(0.3f)
            .OnComplete(operation =>
            {
                OnCompleteRebind(operation);
            })
            .OnCancel(operation => OnCompleteRebind(operation))
            .OnApplyBinding((operation, str) => { SetValue(str); OnJoystickOverrideSet?.Invoke(); })
            .Start();

    }
    private void OnCompleteRebind(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();

        ResetLabel();

        _button.interactable = true;
    }
    private void ResetLabel()
    {
        _buttonLabel.text = _overrides.GetOverride(_overrideIndex);
        if (_buttonLabel.text == "") { _buttonLabel.text = "..."; }
    }
}
