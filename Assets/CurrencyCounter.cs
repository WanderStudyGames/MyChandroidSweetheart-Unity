using System.Collections;
using TMPro;
using UnityEngine;
using VInspector;

public class CurrencyCounter : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _textObject;
    [SerializeField] private TMP_Text _differenceTextObject;
    [SerializeField] private Animator _iconAnimator;
    [SerializeField] private SFX _startSFX;
    [SerializeField] private SFX _moveSFX;
    [SerializeField] private SFX _endSFX;
    [SerializeField] private bool _fadeOut;
    [SerializeField, ShowIf("_fadeOut")] private CanvasFader _fader;
    [SerializeField, EndIf] private bool _unscaledTime;
    private AudioSource _startAudioSource;
    private AudioSource _audioSource;
    private int _currency;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _startAudioSource = gameObject.AddComponent<AudioSource>();
        _startAudioSource.playOnAwake = false;
    }
    private void OnEnable()
    {
        Refresh();

        _wallet.OnCurrencyChanged += OnCurrencyChanged;
    }
    private void OnDisable()
    {
        _wallet.OnCurrencyChanged -= OnCurrencyChanged;
    }
    private void Refresh()
    {
        _differenceTextObject.alpha = 0;
        _currency = _wallet.Currency;
        _textObject.text = _currency.ToString("N0");
        _iconAnimator.SetBool("Add", false);
        if (_fadeOut) _fader.gameObject.SetActive(false);
    }
    private void OnCurrencyChanged(int differential)
    {
        StopAllCoroutines();
        if (Time.timeScale == 0 && !_unscaledTime) { Refresh(); return; }
        StartCoroutine(Co_MoveToCurrency());
        IEnumerator Co_MoveToCurrency()
        {
            if (_fadeOut) { _fader.gameObject.SetActive(true); _fader.FadeIn(0); }
            if (_startSFX != null) _startAudioSource.PlaySFX(_startSFX);
            _iconAnimator.SetBool("Add", true);
            float approx = _currency;
            float currencyLastFrame = _currency;

            SetDifference(differential);

            int absoluteDifference = _wallet.Currency - _currency;

            while (_currency != _wallet.Currency)
            {
                float moveAmount = Mathf.Abs(0.02f * absoluteDifference) * 60 * (_unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                approx = Mathf.MoveTowards(approx, _wallet.Currency, Mathf.Max(moveAmount, 0.01f));

                _currency = Mathf.RoundToInt(approx);

                if (_moveSFX != null && _currency != currencyLastFrame && !_audioSource.isPlaying)
                {
                    _audioSource.PlaySFX(_moveSFX);
                }
                currencyLastFrame = _currency;

                _textObject.text = _currency.ToString("N0");

                //Add((int)Mathf.Sign(difference) * Mathf.CeilToInt(Mathf.Abs(difference) * 0.02f * 60 * Time.deltaTime));
                yield return (_unscaledTime) ? null : new WaitForSeconds(Time.deltaTime);
            }
            if (_endSFX != null)
            {
                yield return new WaitUntil(() => !_audioSource.isPlaying);
                _audioSource.PlaySFX(_endSFX);
            }
            _iconAnimator.SetBool("Add", false);
            yield return (_unscaledTime) ? new WaitForSecondsRT(6) : new WaitForSeconds(6);
            if (_fadeOut) _fader.FadeOut(1);
        }
    }
    private void SetDifference(int number)
    {
        StartCoroutine(Co_Difference());
        IEnumerator Co_Difference()
        {
            var differenceSign = (number > 0) ? "+" : "";
            _differenceTextObject.text = differenceSign + number;
            _differenceTextObject.alpha = 1;
            yield return ExtensionMethods.Co_FadeFloat(0.5f, Vector2.right, fl => _differenceTextObject.alpha = fl, _unscaledTime);
        }
    }
}
