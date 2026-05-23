using UnityEngine;

public class ShaderKeywordLerp : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _duration = 1;
    [SerializeField] private string _keyword;
    private int _id;
    private void Awake()
    {
        _id = Shader.PropertyToID(_keyword);
    }
    public void LerpToOne(float duration)
    {
        StopAllCoroutines();
        float current = _renderer.material.GetFloat(_id);
        if (gameObject.activeSelf)
            StartCoroutine(ExtensionMethods.Co_FadeFloat(duration, new(current, 1), fl =>
            {
                _renderer.material.SetFloat(_id, fl);
            }));
    }
    public void LerpToOne(string keyword)
    {
        StopAllCoroutines();
        int id = Shader.PropertyToID(keyword);

        float current = _renderer.material.GetFloat(id);
        if (gameObject.activeSelf)
            StartCoroutine(ExtensionMethods.Co_FadeFloat(_duration, new(current, 1), fl =>
            {
                _renderer.material.SetFloat(id, fl);
            }));
    }
    public void LerpToZero(float duration)
    {
        StopAllCoroutines();
        float current = _renderer.material.GetFloat(_id);
        if (gameObject.activeSelf)
            StartCoroutine(ExtensionMethods.Co_FadeFloat(duration, new(current, 0), fl =>
            {
                _renderer.material.SetFloat(_id, fl);
            }));
    }
    public void LerpToZero(string keyword)
    {
        StopAllCoroutines();
        int id = Shader.PropertyToID(keyword);

        float current = _renderer.material.GetFloat(id);
        if (gameObject.activeSelf)
            StartCoroutine(ExtensionMethods.Co_FadeFloat(_duration, new(current, 0), fl =>
            {
                _renderer.material.SetFloat(id, fl);
            }));
    }
}
