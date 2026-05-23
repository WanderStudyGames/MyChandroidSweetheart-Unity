using TMPro;

using UnityEngine;

public class FabricIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private static FabricIndicator _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static void Show(string text)
    {
        if (_instance != null)
        {
            _instance._text.text = text;
            _instance._text.gameObject.SetActive(true);
        }
    }
}
