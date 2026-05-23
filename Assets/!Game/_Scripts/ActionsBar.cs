using UnityEngine;
using UnityEngine.UI;

public class ActionsBar : MonoBehaviour
{
    [SerializeField] private Image _primaryImage;
    [SerializeField] private Image _secondaryImage;
    [SerializeField] private Sprite _defaultInteractSprite;

    [SerializeField] private GameObject _secondaryEntry;
    [SerializeField] private GameObject _primaryEntry;
    [SerializeField] private GameObject _commandEntry;
    [SerializeField] private GameObject _rewireDisableEntry;

    private void DisableAll()
    {
        _secondaryEntry.SetActive(false);
        _primaryEntry.SetActive(false);
        _commandEntry.SetActive(false);
        _rewireDisableEntry.SetActive(false);
    }
    private void Start()
    {
        DisableAll();
    }
    private void OnEnable()
    {
        PlayerLookSelector.OnPrimaryHover += OnPrimaryHover;
        PlayerLookSelector.OnPrimaryStopHover += OnPrimaryStopHover;
        PlayerLookSelector.OnSecondaryHover += OnSecondaryHover;
        PlayerLookSelector.OnSecondaryStopHover += OnSecondaryStopHover;

        Commands.OnHoverCompanionInteractible += OnCommandHover;
        Commands.OnStopHoverCompanionInteractible += OnCommandStopHover;

        PlayerStates.Rewiring.OnStateEnableEvent += OnRewireStart;
        PlayerStates.Rewiring.OnStateDisableEvent += OnRewireEnd;
    }
    private void OnDisable()
    {
        DisableAll();
        PlayerLookSelector.OnPrimaryHover -= OnPrimaryHover;
        PlayerLookSelector.OnPrimaryStopHover -= OnPrimaryStopHover;
        PlayerLookSelector.OnSecondaryHover -= OnSecondaryHover;
        PlayerLookSelector.OnSecondaryStopHover -= OnSecondaryStopHover;

        Commands.OnHoverCompanionInteractible -= OnCommandHover;
        Commands.OnStopHoverCompanionInteractible -= OnCommandStopHover;

        PlayerStates.Rewiring.OnStateEnableEvent -= OnRewireStart;
        PlayerStates.Rewiring.OnStateDisableEvent -= OnRewireEnd;
    }
    private void OnPrimaryHover(Sprite s)
    {
        if (s == null) _primaryImage.sprite = _defaultInteractSprite;
        else _primaryImage.sprite = s;
        _primaryEntry.SetActive(true);
    }
    private void OnPrimaryStopHover() { _primaryEntry.SetActive(false); }
    private void OnSecondaryHover(Sprite s)
    {
        _secondaryImage.sprite = s;
        _secondaryEntry.SetActive(true);
    }
    private void OnSecondaryStopHover() { _secondaryEntry.SetActive(false); }
    private void OnCommandHover() { _commandEntry.SetActive(true); }
    private void OnCommandStopHover() { _commandEntry.SetActive(false); }
    private void OnRewireStart() { _rewireDisableEntry.SetActive(true); }
    private void OnRewireEnd() { _rewireDisableEntry.SetActive(false); }

}
