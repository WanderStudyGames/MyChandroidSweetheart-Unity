using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputActionBindingUtility
{
    public static bool TryGetActiveBinding(string activeControlScheme, InputAction inputAction, out InputBinding binding, out int bindingIndex)
    {
        binding = default;
        bindingIndex = 0;
        if (string.IsNullOrEmpty(activeControlScheme)) return false;
        for (int i = 0; i < inputAction.bindings.Count; i++)
        {
            if (inputAction.bindings[i].groups.Contains(activeControlScheme))
            {
                // If not on the specific Keyboard+Mouse control scheme, skip bindings that are for Mouse
                if (!activeControlScheme.Contains("Mouse") && inputAction.bindings[i].effectivePath.Contains("Mouse")) continue;
                binding = inputAction.bindings[i];
                bindingIndex = i;
                if (binding.isPartOfComposite) { bindingIndex -= 1; binding = inputAction.bindings[bindingIndex]; }
                return true;
            }
        }
        return false;
    }

    public static InputBinding[] GetCompositeBindings(InputAction action, int bindingIndex)
    {
        if (!action.bindings[bindingIndex].isComposite || action.bindings[bindingIndex].isPartOfComposite) return new InputBinding[0];
        List<InputBinding> comps = new();
        bindingIndex++;
        while (bindingIndex < action.bindings.Count && action.bindings[bindingIndex].isPartOfComposite)
        {
            comps.Add(action.bindings[bindingIndex]);
            bindingIndex++;
        }
        return comps.ToArray();
    }
    public static bool CanIterateComposite(InputAction action, int bindingIndex) { return bindingIndex + 1 < action.bindings.Count && action.bindings[bindingIndex + 1].isPartOfComposite; }
}