using UnityEngine;
using UnityEngine.UI;

public class DialogueIndicator : MonoBehaviour
{
    private static DialogueIndicator instance;
    [Dependency][SerializeField] private Sprite dialogueImage;
    [Dependency][SerializeField] private Sprite interactImage;
    [Dependency][SerializeField] private Image image;
    [Dependency][SerializeField] private Image crosshair;
    bool inDialogue;
    public static void SetIcon(Sprite sprite)
    {
        instance.image.sprite = sprite;
    }
    private void Awake()
    {
        image.enabled = false;
        instance = this;
    }
    private void OnEnable()
    {
        PlayerLookSelector.OnPrimaryHover += Show;
        PlayerLookSelector.OnPrimaryStopHover += Hide;
        PlayerStateManager.OnStateChanged += OnStateChanged;

    }
    private void OnDisable()
    {
        PlayerLookSelector.OnPrimaryHover -= Show;
        PlayerLookSelector.OnPrimaryStopHover -= Hide;
        PlayerStateManager.OnStateChanged -= OnStateChanged;
    }
    private void Show(Sprite sprite)
    {

        if (sprite == null) image.sprite = interactImage;
        else image.sprite = sprite;
        Show();
    }

    private void Show()
    {
        if (!GlobalPlayerInput.Instance.actions["Primary"].enabled) return;
        //if (inDialogue) return;
        image.enabled = true;
        crosshair.enabled = false;
    }
    private void Hide()
    {
        image.enabled = false;
        crosshair.enabled = true;
    }
    private void OnStateChanged(PlayerState state)
    {
        if (!GlobalPlayerInput.Instance.actions["Primary"].enabled) Hide();
        //if (state == PlayerStates.Scanner || state == PlayerStates.Dialogue || state == PlayerStates.DialogueFrozen || state == PlayerStates.Tablet) { Hide(); inDialogue = true; }
        //else { inDialogue = false; }
    }
}
