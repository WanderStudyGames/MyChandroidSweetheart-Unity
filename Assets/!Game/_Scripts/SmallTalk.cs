using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VInspector;

public class SmallTalk : MonoBehaviour
{
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private Renderer _backgroundPanelRenderer;
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private Vector3 _fadeTimes = Vector3.one;
    [SerializeField] private float _charactersPerSecond = 0.03f;
    [SerializeField] private SFX _startDialogueSFX;
    [SerializeField] private SFX _characterPrintSFX;
    [SerializeField][TextData("SmallTalkTexts/", "SmallTalk_")] private TextAsset _textAsset;
    [SerializeField] private UnityEvent _onEnable;
    [SerializeField] private UnityEvent _onDisable;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();

    }
    [Button]
    private void OnValidate()
    {
        if (_textAsset == null) return;
        _textMeshPro.text = _textAsset.text;
        if (_backgroundPanelRenderer != null)
            _backgroundPanelRenderer.sharedMaterial.color = _backgroundColor;
    }
    private void OnEnable()
    {
        _onEnable.Invoke();
        _textMeshPro.text = "";
        if (_backgroundPanelRenderer != null)
            _backgroundPanelRenderer.material.color = _backgroundColor;
        if (_textMeshPro != null)
            StartCoroutine(Co_Animate());
        IEnumerator Co_Animate()
        {
            transform.localScale = Vector3.zero;
            yield break;
            _startDialogueSFX?.PlayAtPoint(transform.position);

            yield return ExtensionMethods.Co_FadeFloat(_fadeTimes.x, Vector2.up, fl =>
            {
                transform.localScale = Vector3.one * fl;
            });
            yield return TextScroll.Scroll(StringFunctions.ProcessGameText(_textAsset.text), _charactersPerSecond, str =>
            {
                _textMeshPro.text = str;
                if (!_audioSource.isPlaying && _characterPrintSFX != null) { _audioSource.PlaySFX(_characterPrintSFX); }
            }, () => { });
            yield return new WaitForSeconds(_fadeTimes.y);
            yield return ExtensionMethods.Co_FadeFloat(_fadeTimes.z, Vector2.right, fl =>
            {
                transform.localScale = Vector3.one * fl;
            });
            gameObject.SetActive(false);
        }
    }
    public void DisableQuick()
    {
        if (!isActiveAndEnabled) return;
        StopAllCoroutines();
        StartCoroutine(Co_Stop());
        IEnumerator Co_Stop()
        {
            yield return ExtensionMethods.Co_FadeFloat(0.2f, new(_audioSource.volume, 0), fl => _audioSource.volume = fl);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        _audioSource.volume = 0;
        _onDisable.Invoke();
    }
}
