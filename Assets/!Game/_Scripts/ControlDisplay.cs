using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class ControlDisplay : MonoBehaviour
{

    [Dependency][SerializeField] private KeyboardIcons _keyboardIcons;
    [SerializeField] private Sprite _emptyKeyIcon;
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private bool _autoDetectControllerType = true;
    [SerializeField, HideIf("_autoDetectControllerType"), Variants("Xbox", "Switch", "PlayStation")] private string _controllerIconSet;
    [SerializeField, EndIf] private bool _autoDetectBinding = true;
    [SerializeField, HideIf("_autoDetectBinding"), Min(0)] private int _selectedBinding;
#if UNITY_EDITOR
    [SerializeField, ReadOnly] private string _bindingName = "";
#endif
    public void SetActiveBinding(int binding) { _selectedBinding = binding; UpdateDisplay(); }
    public void SetActionReference(InputActionReference reference)
    {
        _actionReference = reference;
        UpdateDisplay();
    }

    [SerializeField, EndIf] private Color _color = Color.white;
    public void SetColor(Color color)
    {
        _color = color;
        _actionIcon.color = color;
    }

    [SerializeField] private ActionIcon _actionIcon;



    private void Awake()
    {
        GlobalPlayerInput.OnControlsChanged += ControlsChange;
        KeyboardIcons.OnIconTypeChanged += UpdateDisplay;
        ActionRebind.OnRebindComplete += UpdateDisplay;
        JoystickControlPathOverride.OnJoystickOverrideSet += UpdateDisplay;
        UpdateDisplay();
    }
    private void OnValidate()
    {
        if (_actionReference == null) return;
        if (_selectedBinding >= _actionReference.action.bindings.Count)
            _selectedBinding = _actionReference.action.bindings.Count - 1;
#if UNITY_EDITOR
        _bindingName = _actionReference.action.bindings[_selectedBinding].path;
#endif
        UpdateDisplay();
    }
    private void OnEnable()
    {
        UpdateDisplay();
    }
    private void OnDestroy()
    {
        ActionRebind.OnRebindComplete -= UpdateDisplay;
        KeyboardIcons.OnIconTypeChanged -= UpdateDisplay;
        JoystickControlPathOverride.OnJoystickOverrideSet -= UpdateDisplay;
        GlobalPlayerInput.OnControlsChanged -= ControlsChange;
    }
    public void ControlsChange(string scheme)
    {
        InputSystem.FlushDisconnectedDevices();
        if (_autoDetectControllerType) _controllerIconSet = scheme;
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        if (_actionReference == null) return;
        if (_actionIcon == null) return;
        if (!isActiveAndEnabled) return;

        if (_autoDetectControllerType) { _controllerIconSet = GlobalPlayerInput.ControlScheme; }

        InputBinding binding = _actionReference.action.bindings[_selectedBinding];
        int bindingIndex = _selectedBinding;

        if (_autoDetectBinding && InputActionBindingUtility.TryGetActiveBinding(_controllerIconSet, _actionReference.action, out InputBinding bin, out int index))
        {
            binding = bin;
            bindingIndex = index;
        }

        if (binding.isComposite)
        {
            StartCompositeCycle(_keyboardIcons, InputActionBindingUtility.GetCompositeBindings(_actionReference.action, bindingIndex));
            return;
        }

        StopAllCoroutines();
        DisplayIcon(_keyboardIcons, binding);
    }
    private void StartCompositeCycle(KeyboardIcons icons, InputBinding[] composites)
    {
        StopAllCoroutines();
        StartCoroutine(Co_CompositeCycle());
        IEnumerator Co_CompositeCycle()
        {
            int i = 0;
            while (true)
            {
                if (i == composites.Length) i = 0;
                DisplayIcon(icons, composites[i]);
                yield return new WaitForSecondsRealtime(1f);
                i++;
            }
        }
    }
    public void DisplayIcon(KeyboardIcons icons, InputBinding binding)
    {
        if (icons.TryGetSprite(binding.effectivePath, _controllerIconSet, out BindingImage bindingImage))
        {
            var col = Color.white;
            if (!bindingImage.ignoreTint) { col = _color; }
            _actionIcon.Set(bindingImage.sprite, "", col);
            return;
        }

        var bindingText = binding.ToDisplayString();
        var text = bindingText.Substring(bindingText.IndexOf('/') + 1).Capitalize();
        if (text == "Escape") text = "Esc";

        _actionIcon.Set(_emptyKeyIcon, text, _color);
    }


}
