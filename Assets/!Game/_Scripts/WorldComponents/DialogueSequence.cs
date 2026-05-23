using System.Collections;
using UnityEngine;

public class DialogueSequence : MonoBehaviour
{
    [BigTitle("Dialogue Sequence")]
    [SerializeField] private DialogueData[] _dialogues;
    private int _index = 0;
    private bool _talked;
    private void Awake()
    {
        foreach (var dialogue in _dialogues)
        {
            if (dialogue.DialogueComponents == null) Debug.LogError($"{nameof(DialogueSequence)}: Must provide dialogue components! (index: {dialogue.TextAsset.name})", gameObject);
            if (dialogue.DialogueComponents.Camera != null) dialogue.DialogueComponents.Camera.enabled = false;

            if (dialogue.AlternativeCamera != null) dialogue.AlternativeCamera.enabled = false;
        }
    }
    public void StartSequence()
    {
        return;
        StopAllCoroutines();
        StartCoroutine(Co_StartSequence());
        IEnumerator Co_StartSequence()
        {
            yield return null;
            _index = 0;
            if (_index >= _dialogues.Length) yield break;
            Speak();
        }

    }
    public void StartSequence(int i)
    {
        StopAllCoroutines();
        StartCoroutine(Co_StartSequence());
        IEnumerator Co_StartSequence()
        {
            yield return null;
            if (i >= _dialogues.Length || i < 0)
            {
                Debug.LogWarning($"{nameof(DialogueSequence)}: Index out of range!", gameObject);
                yield break;
            }
            _index = i;
            Speak();
        }
    }
    private void Speak()
    {
        var dia = _dialogues[_index];

        dia.OnFinish += OnFinish;

        bool endOfSequence = _index == _dialogues.Length - 1;

        DialogueUI.Speak(_dialogues[_index], canSkip: _talked, disableUIAfterwards: endOfSequence);
    }
    private void OnFinish()
    {
        _dialogues[_index].OnFinish -= OnFinish;
        _index++;
        if (_index >= _dialogues.Length)
        {
            _index = 0;
            _talked = true;
        }
        else Speak();
    }
}
