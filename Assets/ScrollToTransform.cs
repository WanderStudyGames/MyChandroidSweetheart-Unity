using UnityEngine;
using UnityEngine.UI;

public class ScrollToTransform : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _target;
    // Start is called before the first frame update
    //IEnumerator Start()
    //{
    //    yield return null;
    //    ScrollToTarget(_target);

    //}
    public void ScrollToTarget(RectTransform target)
    {
        _scrollRect.normalizedPosition = Vector2.zero;
        var viewportPos = _scrollRect.viewport.position;
        _content.position = viewportPos - target.position + _content.position;
    }

}
