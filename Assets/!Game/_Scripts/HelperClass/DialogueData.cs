using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueData
{
    private enum ProviderType { Local, Persistent }
    [TextData("DialogueTexts/", "Dialogue_")] public TextAsset TextAsset;
    public SFX StartSFX;
    [field: SerializeField] public bool InstantHeadTurn { get; private set; } = false;
    [field: SerializeField] public Camera AlternativeCamera { get; private set; }
    [Separator]

    [Header("Profile")]

    [SerializeField] private CharacterProfile _diaProfileGlobal;
    [SerializeField] private CharacterDialogueProfileLocal _diaProfileLocal;

    [Header("Dialogue Components")]

    [SerializeField] private DialogueComponents _diaComponentsGlobal;
    [SerializeField] private DialogueComponentsProvider _diaComponentsLocal;

    [Separator]
    [SerializeField] private UnityEvent _onFinish;
    public event Action OnFinish;
    public void FinishAction()
    {
        _onFinish.Invoke();
        OnFinish?.Invoke();
    }
    public ICharacterProfile CharacterProfile
    {
        get { return (_diaProfileGlobal != null) ? _diaProfileGlobal : _diaProfileLocal; }
    }
    public IDialogueComponents DialogueComponents
    {
        get
        {
            if (_diaComponentsGlobal == null && _diaComponentsLocal == null)
            {
                Debug.LogError($"{nameof(DialogueData)}: Must provide dialogue components!");
            }
            return (_diaComponentsGlobal != null) ? _diaComponentsGlobal : _diaComponentsLocal;
        }
    }
}

