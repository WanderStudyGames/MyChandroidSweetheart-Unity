using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ActionRebind : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private string _displayName;
    //[SerializeField, Variants("Keyboard Mouse", "Gamepad", "Joystick")] private string _controlScheme;
    [SerializeField, Min(0)] private int _bindingIndex;
#if UNITY_EDITOR
    [SerializeField, ReadOnly] private string _bindingName = "";
#endif
    [SerializeField] private ControlDisplay _controlDisplay;
    [SerializeField] private TMP_Text _actionLabel;
    [SerializeField] private TMP_Text _rebindingLabel;

    [SerializeField] private bool _axisControl;

    private static InputActionRebindingExtensions.RebindingOperation _operation;

    public static event Action OnRebindComplete;
    private void Start()
    {
        _controlDisplay.SetActionReference(_actionReference);
        _controlDisplay.SetActiveBinding(_bindingIndex);

        ResetActionText();
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_actionReference == null) Debug.LogError("actionreference null", this.gameObject);
        if (_bindingIndex >= _actionReference.action.bindings.Count)
            _bindingIndex = _actionReference.action.bindings.Count - 1;
        _bindingName = _actionReference.action.bindings[_bindingIndex].path;
    }
#endif
    private void OnDisable()
    {
        if (_operation != null && _operation.started) { _operation.Cancel(); }
    }
    public void ResetBinding()
    {
        if (_actionReference.action.bindings[_bindingIndex].isComposite)
        {
            for (int i = _bindingIndex + 1; i < _actionReference.action.bindings.Count; i++)
            {
                if (!_actionReference.action.bindings[i].isPartOfComposite) { break; }
                _actionReference.action.RemoveBindingOverride(i);
            }
        }
        else
        {
            _actionReference.action.RemoveBindingOverride(_bindingIndex);
            Debug.Log(_actionReference.action.bindings[_bindingIndex].path);
        }
        OnRebindComplete?.Invoke();
    }
    public void StartRebind()
    {
        if (_operation != null && _operation.started) return;
        StartRebind(_bindingIndex);
    }
    private void StartRebind(int bindingIndex)
    {
        var action = _actionReference.action;
        _controlDisplay.gameObject.SetActive(false);
        _rebindingLabel.gameObject.SetActive(true);

        bool actionWasEnabled = action.enabled;
        if (actionWasEnabled) action.Disable();


        if (action.bindings[bindingIndex].isComposite && InputActionBindingUtility.CanIterateComposite(action, bindingIndex))
        {
            bindingIndex++;
            _actionLabel.text = $"Press {action.bindings[bindingIndex].name.Capitalize()}";
        }
        else
        {
            _actionLabel.text = "Esc to cancel";
        }
        PerformInteractiveRebinding(_actionReference.action, bindingIndex, actionWasEnabled);

    }
    private void PerformInteractiveRebinding(InputAction action, int bindingIndex, bool enableActionOnComplete)
    {
        _operation = action.PerformInteractiveRebinding()
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Keyboard>/backquote")
            .WithControlsExcluding("<Keyboard>/enter")
            .WithControlsExcluding("<Keyboard>/p")
            .WithControlsExcluding("<Keyboard>/anyKey")
            .WithTimeout(5)
            .WithBindingGroup(action.bindings[bindingIndex].groups);

        if (_axisControl) { _operation = _operation.WithExpectedControlType<AxisControl>(); }
        else { _operation = _operation.WithExpectedControlType<ButtonControl>(); }

        _operation = _operation
            .WithTargetBinding(bindingIndex)
            .OnMatchWaitForAnother(0.3f)
            .OnComplete(operation =>
            {
                if (action.bindings[bindingIndex].isPartOfComposite && InputActionBindingUtility.CanIterateComposite(action, bindingIndex))
                {
                    operation.Dispose();
                    _actionLabel.text = $"Press {action.bindings[bindingIndex + 1].name.Capitalize()}";
                    PerformInteractiveRebinding(action, bindingIndex + 1, enableActionOnComplete);
                }
                else
                {
                    OnCompleteRebind(operation, enableActionOnComplete);
                }
            })
            .OnCancel(operation => OnCompleteRebind(operation, enableActionOnComplete))
            .Start();
    }
    private void OnCompleteRebind(InputActionRebindingExtensions.RebindingOperation operation, bool enableAction)
    {
        operation.Dispose();

        _controlDisplay.gameObject.SetActive(true);
        _rebindingLabel.gameObject.SetActive(false);

        ResetActionText();

        if (enableAction) _actionReference.action.Enable();
        OnRebindComplete?.Invoke();
    }
    private void ResetActionText()
    {
        if (_displayName != "") { _actionLabel.text = _displayName; return; }
        if (_actionReference.action.bindings[_bindingIndex].isPartOfComposite)
        {
            _actionLabel.text = $"{_actionReference.action.name} {_actionReference.action.bindings[_bindingIndex].name.Capitalize()}";
        }
        else
            _actionLabel.text = _actionReference.action.name;
    }
}
