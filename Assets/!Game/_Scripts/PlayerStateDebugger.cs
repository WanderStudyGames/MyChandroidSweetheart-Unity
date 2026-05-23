using TMPro;
using UnityEngine;

public class PlayerStateDebugger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    [ContextMenu("bleh")]
    private void Bleh()
    {
        if (PlayerStateManager.State == null) return;
        tmp.text = PlayerStateManager.State.name;
    }
    private void Update()
    {
        Bleh();
    }
}
