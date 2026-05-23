using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    [SerializeField] private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start()
    {

    }
    [ContextMenu("Bleh")]
    void Set()
    {
        tmp.text = "";
        foreach (InputAction a in _playerInput.actions)
        {
            string str = $"{a.actionMap.name}: {a.enabled}\n";
            if (!tmp.text.Contains(str)) tmp.text += str;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Set();
    }
}
