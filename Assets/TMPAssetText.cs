using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_Text))]
public class TMPAssetText : MonoBehaviour
{
    [TextData("TMPTexts/", "TMP_")]
    [SerializeField] private TextAsset _textAsset;
    [SerializeField] private UnityEvent<string> _onUpdate;
    private TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        UpdateText();
    }
    public void UpdateText()
    {
        if (_textAsset == null || text == null) return;
        text.text = StringFunctions.ProcessGameText(_textAsset.text);
        _onUpdate.Invoke(StringFunctions.ProcessGameText(_textAsset.text));
    }
    [ContextMenu("OnValidate()")]
    private void OnValidate()
    {
        var txt = GetComponent<TMP_Text>();
        if (_textAsset == null) return;
        txt.text = StringFunctions.ProcessGameText(_textAsset.text);
        _onUpdate.Invoke(StringFunctions.ProcessGameText(_textAsset.text));
    }
}
