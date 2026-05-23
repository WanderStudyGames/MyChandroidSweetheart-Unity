using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class TimelineSkip : MonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private float _destinationTime;
    [SerializeField] private InputActionReference _skipButton;
    [SerializeField] private Animator _skipDisplay;
    private bool _wasEnabled;
    public void Skip()
    {
        _timeline.time = _destinationTime;
        if (!_wasEnabled) _skipButton.action.Disable();
    }
    private void OnEnable()
    {
        _skipDisplay.gameObject.SetActive(false);
        _wasEnabled = _skipButton.action.enabled;
        _skipButton.action.Enable();
        _skipButton.action.Link(StartSkip);
    }
    private void OnDisable()
    {
        _skipButton.action.UnLink(StartSkip);
    }
    private void StartSkip(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            _skipDisplay.gameObject.SetActive(true);
        }
        if (ctx.canceled)
        {
            _skipDisplay.gameObject.SetActive(false);
        }
    }
}
