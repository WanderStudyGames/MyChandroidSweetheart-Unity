using TMPro;
using UnityEngine;

public class HeaderText : MonoBehaviour
{
    public string Text => textAsset.text;
    [SerializeField][TextData("HeaderTexts/", "Header_")] private TextAsset textAsset;
    [SerializeField] private TextAlignmentOptions textAlignment = TextAlignmentOptions.Top;

    public void ShowHeader()
    {
        UIManager.HeaderAlignment = textAlignment;
        UIManager.Header(textAsset.text);
    }
    public void HideHeader()
    {
        UIManager.HideHeader();
    }
}
