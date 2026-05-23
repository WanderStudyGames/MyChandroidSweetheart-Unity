using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    // Start is called before the first frame update
    void Awake()
    {
        _text.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
