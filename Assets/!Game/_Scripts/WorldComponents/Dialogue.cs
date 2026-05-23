using System;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;

    [SerializeField] private SFX dialogueSFX;

    [SerializeField] private CharacterProfile characterDialogueProfile;
    [SerializeField] private Camera diaCamera;
    public Camera Camera => diaCamera;
    [SerializeField] private GameObject focusObject;
    [SerializeField] private Color color = Color.gray;
    public Color Color => color;

    [SerializeField] private Dialogue nextDialogue;
    public Dialogue NextDialogue => nextDialogue;
    [SerializeField] private float freezePlayerLerpSpeed = 5f;
    [SerializeField] private UnityEvent executeOnFinish;
    private bool inDialogue;

    public bool Completed { get; set; }


    private void Awake()
    {
        if (diaCamera != null) { diaCamera.enabled = false; }
    }
    public void OnDialogueFinish()
    {

        inDialogue = false;

        if (diaCamera != null) { diaCamera.enabled = false; }
        if (nextDialogue == null)
        {
            PlayerManager.SetDialogueRestrictions(null, 200f);
            PlayerStateManager.SwitchState(PlayerStates.Default);
        }
        else nextDialogue.Speak();

        executeOnFinish.Invoke();
    }

    [ContextMenu("Speak()")]
    public void Speak()
    {
        return;
        //if (!isActiveAndEnabled) return;

        if (characterDialogueProfile != null)
        {
            color = characterDialogueProfile.Color;
            //if (characterDialogueProfile.Scope == CharacterProfile.Scopes.Persistent)
            //{
            //    focusObject = characterDialogueProfile.PersistentDialogueFields.GameObject;
            //    diaCamera = characterDialogueProfile.PersistentDialogueFields.Camera;
            //}
        }
        if (focusObject == null) focusObject = gameObject;

        //PlayerManager.SetDialogueRestrictions(focusObject, 5f);

        if (!inDialogue)
        {
            inDialogue = true;
            if (dialogueSFX != null) SFX.PlayAtPoint(dialogueSFX, focusObject.transform.position);
            //controls get changed
            OnCharacterSpeak?.Invoke(this);
            if (diaCamera == null) return;
            diaCamera.enabled = true;
        }

    }
    public static event Action<Dialogue> OnCharacterSpeak;

    public string Text => textAsset.text.Replace("<playername>", $"<color=#96D3FF>{SetPlayerName.PlayerName}</color>").Replace("<companionname>", $"<color=#FF80FF>{CompanionData.CompanionName}</color>");
}
