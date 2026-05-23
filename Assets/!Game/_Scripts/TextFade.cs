using System.Collections;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TextFade : MonoBehaviour
{
    [SerializeField] private Vector3 fadeTime = Vector3.one;
    private TMP_Text text;
    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        StartCoroutine(Co_Fade());
    }

    private IEnumerator Co_Fade()
    {
        yield return ExtensionMethods.Co_FadeFloat(fadeTime.x, new(0, 1), fl =>
        {
            var c = text.color;
            c.a = fl;
            text.color = c;
        });
        yield return new WaitForSeconds(fadeTime.y);
        yield return ExtensionMethods.Co_FadeFloat(fadeTime.z, new(1, 0), fl =>
        {
            var c = text.color;
            c.a = fl;
            text.color = c;
        });
    }
}
