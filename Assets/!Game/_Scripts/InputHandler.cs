using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    //public static void Link(string actionName, Action<InputAction.CallbackContext> action)
    //{
    //    PlayerInput.actions[actionName].started += action;
    //    PlayerInput.actions[actionName].performed += action;
    //    PlayerInput.actions[actionName].canceled += action;
    //}
    //public static void UnLink(string actionName, Action<InputAction.CallbackContext> action)
    //{
    //    PlayerInput.actions[actionName].started += action;
    //    PlayerInput.actions[actionName].performed += action;
    //    PlayerInput.actions[actionName].canceled += action;
    //}
    //public void SetMaps(InputActionMap[] inputActionMaps)
    //{
    //    foreach (InputActionMap map in inputActionMaps)
    //    {
    //        if (!map.enabled)
    //        {
    //            map.Enable();
    //            Debug.Log("enabled " + map.name);
    //        }
    //    }
    //    List<InputActionMap> mapsToDisable = new();
    //    foreach (InputAction action in _playerInput.actions)
    //    {
    //        if (!inputActionMaps.Contains(action.actionMap) && !mapsToDisable.Contains(action.actionMap))
    //        {
    //            mapsToDisable.Add(action.actionMap);
    //        }
    //    }
    //    foreach (InputActionMap map in mapsToDisable)
    //    {
    //        map.Disable();
    //        Debug.Log("disabled " + map.name);
    //    }

    //}

    //public class InputProfiles
    //{
    //    private static InputActionAsset asset => PlayerInput.actions;
    //    public static InputActionMap[] Default => new InputActionMap[] { asset.FindActionMap("FPS"), asset.FindActionMap("Default") };
    //    public static InputActionMap[] Dialogue => new InputActionMap[] { asset.FindActionMap("FPS"), asset.FindActionMap("Default"), asset.FindActionMap("Dialogue") };
    //    public static InputActionMap[] Scanner => new InputActionMap[] { asset.FindActionMap("FPS"), asset.FindActionMap("Scanner") };
    //    public static InputActionMap[] Scanning => new InputActionMap[] { asset.FindActionMap("Scanning") };
    //    public static InputActionMap[] Carrying => new InputActionMap[] { asset.FindActionMap("FPS"), asset.FindActionMap("Carrying") };
    //    public static InputActionMap[] DialogueFrozen => new InputActionMap[] { asset.FindActionMap("Dialogue") };
    //    public static InputActionMap[] Menu => new InputActionMap[] { asset.FindActionMap("Menu") };
    //}
}
public struct InputMaps
{
    public readonly static string[] Default = { "Default" };
    public readonly static string[] Dialogue = { "Default" };
    public readonly static string[] Menu = { "Menu" };
}