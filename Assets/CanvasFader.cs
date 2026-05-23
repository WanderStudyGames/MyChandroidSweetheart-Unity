using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _disableAfterFadeOut;
    public void FadeIn(int seconds)
    {
        StopAllCoroutines();
        StartCoroutine(ExtensionMethods.Co_FadeFloat(seconds, new(_canvasGroup.alpha, 1), fl =>
        {
            _canvasGroup.alpha = fl;
        }, true)); ;
    }
    public void FadeOut(int seconds)
    {
        if (!isActiveAndEnabled) { _canvasGroup.alpha = 0; if (_disableAfterFadeOut) { gameObject.SetActive(false); } return; }
        StopAllCoroutines();
        StartCoroutine(ExtensionMethods.Co_FadeFloat(seconds, new(_canvasGroup.alpha, 0), fl =>
        {
            _canvasGroup.alpha = fl;
            if (fl == 0 && _disableAfterFadeOut)
            {
                gameObject.SetActive(false);
            }
        }, true));
    }
}
