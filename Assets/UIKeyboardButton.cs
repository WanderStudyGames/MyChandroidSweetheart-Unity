using UnityEngine;

public class UIKeyboardButton : MonoBehaviour
{
    [SerializeField] UIKeyboard _uiKeyboard;

    public void SendCharacter()
    {
        _uiKeyboard.AddCharacter(gameObject.name[0]);
    }

}
