using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIKeyboard : MonoBehaviour
{
    private string _string = "";
    [SerializeField] private string _placeholder;
    [SerializeField] private TMP_Text _textMesh;
    [SerializeField] private UnityEvent<string> _submit;
    private void OnEnable()
    {
        if (_string == "") { _textMesh.text = _placeholder; }
    }
    public void AddCharacter(char c)
    {
        _string += c;
        _textMesh.text = _string;
    }
    public void RemoveCharacter()
    {
        if (_string.Length > 0)
        {
            _string = _string[..^1];
            if (_string != "")
            {
                _textMesh.text = _string;

            }
            else
            {
                _textMesh.text = _placeholder;
            }
        }
    }
    public void SetString(string s)
    {
        _string = s.RemoveTags();
        if (!string.IsNullOrEmpty(s)) { _textMesh.text = s.RemoveTags(); }
        else { _textMesh.text = _placeholder; }
    }
    public void Clear()
    {
        _string = "";
        _textMesh.text = _placeholder;
    }
    public void Submit()
    {
        _submit.Invoke(_string);
    }

}
