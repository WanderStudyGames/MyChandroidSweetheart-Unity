using UnityEngine;
using UnityEngine.UI;

public class SpriteFader : MonoBehaviour
{
    [SerializeField] private float _fadeTime;
    [Dependency][SerializeField] private Graphic _graphic;
    private float _targetAlpha;
    // Start is called before the first frame update
    void Awake()
    {
        _targetAlpha = _graphic.color.a;
    }

    // Update is called once per frame
    void OnEnable()
    {

        StopAllCoroutines();
        StartCoroutine(ExtensionMethods.Co_FadeFloat(_fadeTime, new(0, _targetAlpha), fl =>
        {
            _graphic.color = new(
                _graphic.color.r,
                _graphic.color.g,
                _graphic.color.b,
                fl
                );
        }));
    }
}
