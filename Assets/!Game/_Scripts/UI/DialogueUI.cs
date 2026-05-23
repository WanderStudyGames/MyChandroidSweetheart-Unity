using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : UIComponent
{
    [Dependency][SerializeField] private GameObject _panel;
    [Dependency][SerializeField] private Image panelImage;
    [Dependency][SerializeField] private GameObject nextPageIcon;
    [Dependency][SerializeField] private TextMeshProUGUI textMesh;
    [Dependency][SerializeField] private SFX textScrollSFX;
    [Dependency][SerializeField] private Image _namePanel;
    [Dependency][SerializeField] private TMP_Text _nameText;
    [SerializeField] private int charLimit;
    [SerializeField] private PlayerInput _playerInput;

    private static DialogueUI instance;

    private static Camera _camera;
    private static Camera _altCamera;
    private static Action _onFinish;

    private IEnumerator _scrollCoroutine;
    private static int _pageNumber;
    private static bool _nextPageAvailable;
    private static bool _canSkip;
    private bool _disableUIAfter;
    private AudioSource _textScrollAudioSource;

    private static string[] _pages = { };

    protected override void Awake()
    {
        instance = this;
        _textScrollAudioSource = gameObject.AddComponent<AudioSource>();
        _textScrollAudioSource.playOnAwake = false;
        base.Awake();

    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    private void OnEnable()
    {
        PlayerStates.Scanner.OnStateEnableEvent += LeaveDialogue;
        TextBox.OnTextBoxOpen += CloseDialogue;


    }
    private void OnDisable()
    {
        LeaveDialogue();
        PlayerStates.Scanner.OnStateEnableEvent -= LeaveDialogue;

        TextBox.OnTextBoxOpen -= CloseDialogue;
        PlayerStates.Dialogue.OnPageSkip -= NextPage;
        PlayerStates.DialogueFrozen.OnPageSkip -= NextPage;

    }
    private void Start()
    {
        _panel.SetActive(false);
    }
    void CloseDialogue()
    {
        _disableUIAfter = true;
        LeaveDialogue();
    }
    void LeaveDialogue()
    {

        if (_disableUIAfter)
        {
            _panel.SetActive(false);
        }

        if (_scrollCoroutine != null) StopCoroutine(_scrollCoroutine);

        PlayerManager.SetCameraEnabled(true);
        if (_altCamera != null) _altCamera.enabled = false;
        _altCamera = null;

        if (PlayerStateManager.State == PlayerStates.DialogueFrozen)
        {
            if (_disableUIAfter)
            {
                PlayerStateManager.SwitchState(PlayerStates.Default);
                PlayerManager.SetDialogueRestrictions(null, 200f);
            }
            if (_camera != null) _camera.enabled = false;
            _onFinish?.Invoke();
        }

        PlayerStates.Dialogue.OnPageSkip -= NextPage;
        PlayerStates.DialogueFrozen.OnPageSkip -= NextPage;
    }

    void DisplayText()
    {
        if (_scrollCoroutine != null) StopCoroutine(_scrollCoroutine);
        nextPageIcon.SetActive(false);


        _textScrollAudioSource.PlaySFX(textScrollSFX);

        _nextPageAvailable = false;
        _scrollCoroutine = TextScroll.Scroll(_pages[_pageNumber], 0.015f,
                (text) =>
                {
                    textMesh.text = "    " + text;
                    if (!_textScrollAudioSource.isPlaying) _textScrollAudioSource.Play();
                },
                () =>
                {
                    nextPageIcon.SetActive(true);
                    _nextPageAvailable = true;
                }
            );
        StartCoroutine(_scrollCoroutine);
    }
    void NextPage()
    {
        if (_pageNumber >= _pages.Length - 1 && (_nextPageAvailable || _canSkip))
        {
            LeaveDialogue();
        }
        else if (_pages.Length > 0 && _pageNumber < _pages.Length - 1 && (_nextPageAvailable || _canSkip))
        {
            _pageNumber++;
            DisplayText();
        }

    }

    public static void Speak(DialogueData dialogueData, bool canSkip = false, bool disableUIAfterwards = true)
    {
        if (instance == null) return;
        PlayerStates.Dialogue.OnPageSkip += instance.NextPage;
        PlayerStates.DialogueFrozen.OnPageSkip += instance.NextPage;

        _canSkip = canSkip;
        instance._disableUIAfter = disableUIAfterwards;
        instance._panel.SetActive(true);
        instance.panelImage.color = dialogueData.CharacterProfile.Color;
        instance._namePanel.color = dialogueData.CharacterProfile.NameColor;
        instance._nameText.text = dialogueData.CharacterProfile.Name.Replace("<companionname>", CompanionData.CompanionName);
        instance._namePanel.gameObject.SetActive(dialogueData.CharacterProfile.Name != "");

        _pages = StringFunctions.EvaluateTextPages(StringFunctions.ProcessGameText(dialogueData.TextAsset.text), instance.charLimit);
        _pageNumber = 0;
        _onFinish = dialogueData.FinishAction;
        _camera = dialogueData.DialogueComponents.Camera;
        _altCamera = dialogueData.AlternativeCamera;

        if (_camera != null) _camera.enabled = true;
        if (_altCamera != null)
        {
            PlayerManager.SetCameraEnabled(false);
            _altCamera.enabled = true;
        }


        var speed = dialogueData.InstantHeadTurn ? 1000f : 5f;
        if (dialogueData.DialogueComponents.FocusObject == null) Debug.LogError("Focus object is null!");
        PlayerManager.SetDialogueRestrictions(dialogueData.DialogueComponents.FocusObject, speed);

        instance.DisplayText();
    }
    public override void SetComponentProfile(ComponentProfile profile)
    {
    }
}
