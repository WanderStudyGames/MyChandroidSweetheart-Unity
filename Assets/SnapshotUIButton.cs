using TMPro;
using UnityEngine;

public class SnapshotUIButton : MonoBehaviour
{
    [SerializeField] private SaveDataSnapshot snapshot;
    [SerializeField] private TMP_Text buttonText;
    public void SetData(SaveDataSnapshot newSnapshot, string label)
    {
        snapshot = newSnapshot;
        if (buttonText != null)
        {
            buttonText.text = label;
        }
    }
    public void LoadSnapshot()
    {
        if (snapshot != null)
        {
            snapshot.LoadData();
        }
    }
}
