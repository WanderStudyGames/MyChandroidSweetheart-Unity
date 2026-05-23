using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    private PlayerState _previousState;
    [SerializeField] TMP_Text _text;
    [SerializeField] GameObject _nextPageIcon;
    [SerializeField] Image _panel;
    [SerializeField, Range(1, 500)] private int _charLimit = 100;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] InputActionReference _nextPageAction;
    [SerializeField] private SFX _characterTypeSFX;
    public bool CanSkip;
    [TextData("TextBoxTexts/", "TextBox_")] public TextAsset TextAsset;
    [SerializeField] private UnityEvent _onClose;
    public static event Action OnTextBoxOpen;
    private AudioSource _audioSource;
    public void SetColor(Color color) { _color = color; if (_panel != null) _panel.color = color; }
    private void OnValidate()
    {
        SetColor(_color);
    }
    private string[] _pages;
    private int _pageNumber = 0;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void OnEnable()
    {
        OnTextBoxOpen?.Invoke();
        _previousState = PlayerStateManager.State;
        PlayerStateManager.SwitchState(PlayerStates.DialogueFrozen);

        _pages = StringFunctions.EvaluateTextPages(StringFunctions.ProcessGameText(TextAsset.text), _charLimit);
        _pageNumber = 0;

        PlayerStates.DialogueFrozen.OnPageSkip += NextPage;

        DisplayText();
    }
    private void DisplayText()
    {
        _nextPageIcon.SetActive(false);
        var scroll = TextScroll.Scroll(_pages[_pageNumber], 0.015f, str =>
        {
            _text.text = str;
            if (!_audioSource.isPlaying)
                _audioSource.PlaySFX(_characterTypeSFX);
        }, () =>
        {
            _nextPageIcon.SetActive(true);
        });
        StopAllCoroutines();
        StartCoroutine(scroll);
    }
    private void NextPage()
    {
        if (_pageNumber < _pages.Length - 1)
        {
            _pageNumber++;
            DisplayText();
        }
        else
        {
            Close();
        }
    }
    private void Close()
    {
        _onClose.Invoke();
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        PlayerStateManager.SwitchState(_previousState);
        PlayerStates.DialogueFrozen.OnPageSkip -= NextPage;

    }
}
