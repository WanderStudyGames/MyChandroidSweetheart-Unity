using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScanUIRoot))]
public class ScanUI : UIComponent
{
    [BigTitle("Scan UI")]
    [Dependency][SerializeField] private TextMeshProUGUI textMesh;
    [Dependency][SerializeField] private GameObject nextPageIcon;
    [Dependency][SerializeField] private PanelUnfold _panel;
    [Dependency][SerializeField] private GameObject _progressBarPanel;
    [Dependency][SerializeField] private GameObject _leaveScanIcon;
    [Dependency][SerializeField] private CanvasFader _scannerOverlay;
    [Dependency][SerializeField] private SFXSource scanningSFXSource;

    [Dependency][SerializeField] private TMP_Text _counterTextObject;

    [Dependency][SerializeField] private GameObject _nameGroup;
    [Dependency][SerializeField] private TMP_Text _name;
    [SerializeField] private Image[] _colorableImages;

    [SerializeField] private SFX scanTextScrollSFX;
    [SerializeField] private ScanUIProfile scanUIProfile;

    public override void SetComponentProfile(ComponentProfile profile)
    {
        scanUIProfile = (ScanUIProfile)profile;
    }

    private static ScanUI instance;

    private IEnumerator scrollCoroutine;

    private int pageNumber;
    private string[] _pages = { };
    private bool canSkip;
    private Scan _scan;


    private AudioSource audioSource;
    private AudioSource scrollAudioSource;
    private AudioSource ambientAudioSource;

    protected override void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        scrollAudioSource = gameObject.AddComponent<AudioSource>();
        scrollAudioSource.playOnAwake = false;
        ambientAudioSource = gameObject.AddComponent<AudioSource>();
        ambientAudioSource.playOnAwake = false;

        instance = this;

        base.Awake();
    }
    private void OnEnable()
    {
        PlayerStates.Scanner.OnStateEnableEvent += ScanMode;
        PlayerStates.Default.OnStateEnableEvent += DefaultMode;
        PlayerStates.DialogueFrozen.OnStateEnableEvent += DefaultMode;
        //_playerInput.actions.Link("ScanPageSkip", OnPageSkip);
        PlayerStates.Scanning.OnPageSkip += OnPageSkip;
        PlayerStates.Scanning.OnLeaveScan += OnLeaveScan;
        if (PlayerStateManager.State == PlayerStates.Default) { DefaultMode(); _scannerOverlay.FadeOut(0); }
    }
    private void OnDisable()
    {
        PlayerStates.Scanner.OnStateEnableEvent -= ScanMode;
        PlayerStates.Scanning.OnPageSkip -= OnPageSkip;
        PlayerStates.Scanning.OnLeaveScan -= OnLeaveScan;
        PlayerStates.Default.OnStateEnableEvent -= DefaultMode;
        PlayerStates.DialogueFrozen.OnStateEnableEvent -= DefaultMode;
        //_playerInput.actions.UnLink("ScanPageSkip", OnPageSkip);
    }
    private void DefaultMode()
    {
        _scannerOverlay.FadeOut(1);
        ambientAudioSource.Stop();
    }
    private void ScanMode()
    {

        ambientAudioSource.PlaySFX(scanUIProfile.scanAmbienceSFX);
        _scannerOverlay.gameObject.SetActive(true);
        _scannerOverlay.FadeIn(1);
    }
    private void Start()
    {
        _panel.Close();
        _scannerOverlay.FadeOut(0);
        StopAllCoroutines();

    }

    public static void Scan(Scan scan)
    {
        instance._scan = scan;
        instance._leaveScanIcon.gameObject.SetActive(scan.Completed);
        if (scan.Completed)
        {
            instance.StartCoroutine(Co_WaitAFrame());
        }
        else
        {
            instance.StartProgressBar();
        }



        IEnumerator Co_WaitAFrame()
        {
            yield return null;
            instance.ShowScan();
        }
    }

    public void ShowScan()
    {
        PlayerStateManager.SwitchState(PlayerStates.Scanning);
        scanningSFXSource.Stop();
        _panel.gameObject.SetActive(true);
        pageNumber = 0;
        textMesh.text = "";
        _name.text = "";

        bool hasName = _scan != null && _scan.Name.Length > 0;
        //if (hasName)
        //{
        //    _name.text = _scan.Name.Replace("<companionname>", CompanionData.CompanionName);
        //}
        foreach (var image in _colorableImages)
        {
            image.color = _scan.Color;
        }
        _nameGroup.SetActive(hasName);

        _pages = StringFunctions.EvaluateTextPages(StringFunctions.ProcessGameText(_scan.GetText()), scanUIProfile.CharLimit);
        //_pages[0] = "    " + _pages[0];

        audioSource.PlaySFX(scanUIProfile.scanStartSFX);

        DisplayText();
    }

    private void StartProgressBar()
    {
        _progressBarPanel.SetActive(true);
        scanningSFXSource.Play();
    }
    public static event Action OnDisplayText;
    private void DisplayText()
    {
        if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);

        OnDisplayText?.Invoke();
        _counterTextObject.text = $"{pageNumber + 1}/{_pages.Length}";
        scanUIProfile.OnDisplayTextGE.Raise();
        nextPageIcon.SetActive(false);
        canSkip = _scan.Completed;
        scrollAudioSource.PlaySFX(scanTextScrollSFX);
        IEnumerator Co_DisplayText()
        {

            bool hasName = _scan != null && _scan.Name.Length > 0;
            if (hasName)
            {
                //yield return new WaitForSecondsRT(0.5f);
                yield return TextScroll.Scroll(_scan.Name, scanUIProfile.ScrollDelayName, (text) =>
                {
                    _name.text = text.Replace("<companionname>", CompanionData.CompanionName); if (!scrollAudioSource.isPlaying) scrollAudioSource.Play();
                });
            }
            yield return new WaitForSecondsRT(0.5f);
            yield return TextScroll.Scroll
            (_pages[pageNumber], scanUIProfile.ScrollDelay,
                (text) => { textMesh.text = text; if (!scrollAudioSource.isPlaying) scrollAudioSource.Play(); },
                () => { if (pageNumber <= _pages.Length - 1) { nextPageIcon.SetActive(true); canSkip = true; } }
            );
        }
        scrollCoroutine = Co_DisplayText();
        StartCoroutine(scrollCoroutine);
    }

    public static void ScanClick() { }
    public static void ScanUnclick() { }

    public void OnPageSkip()
    {
        if (!canSkip) return;
        if (_pages.Length <= 0) return;
        if (pageNumber < _pages.Length - 1)
        {
            pageNumber++;
            DisplayText();
            audioSource.PlaySFX(scanUIProfile.scanPageSkipSFX);
        }
        else { LeaveScan(); }
    }
    private void OnLeaveScan()
    {
        LeaveScan();
    }
    public static void LeaveScan()
    {
        if (instance == null) return;
        if (instance.scanningSFXSource != null) instance.scanningSFXSource?.Stop();
        if (instance.scrollCoroutine != null) instance.StopCoroutine(instance.scrollCoroutine);
        if (instance._scan != null && instance._panel.gameObject.activeSelf) { instance.audioSource.PlaySFX(instance.scanUIProfile.scanStopSFX); }
        instance._panel.Close();
        instance._progressBarPanel.SetActive(false);
        if (PlayerStateManager.State == PlayerStates.Scanning)
        {
            instance.StartCoroutine(OneFrameLater());
            IEnumerator OneFrameLater()
            {
                yield return null;
                instance._scan?.ExecuteLeaveScanAction();
                instance._scan = null;
            }
            PlayerStateManager.SwitchState(PlayerStates.Scanner);
        }
    }
}
