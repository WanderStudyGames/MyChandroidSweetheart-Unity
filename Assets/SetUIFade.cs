using UnityEngine;
using UnityEngine.Events;

public class SetUIFade : MonoBehaviour
{
    [SerializeField] private Vector3 _fadeTimes;
    [SerializeField] private Color _color;
    [SerializeField] private UnityEvent _executeAfterFade;
    public void Set()
    {
        UIFade.FadeDurations = _fadeTimes;
        UIFade.FadeColor = _color;
    }
    public void ExecuteAfterFade()
    {
        Set();
        UIFade.ExecuteAfterFade(() =>
        {
            _executeAfterFade.Invoke();
            UIFade.FadeIn();
        }, false);
    }
}
