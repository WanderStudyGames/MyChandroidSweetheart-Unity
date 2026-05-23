using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapshotUIController : MonoBehaviour
{
    [SerializeField] private SnapshotUIButton buttonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private UnityEvent _onSnapshotsEnabled;
    [SerializeField] private UnityEvent _onSnapshotsDisabled;
    [SerializeField] private SaveDataSnapshotProvider snapshotProvider;
    private List<SnapshotUIButton> buttons = new();
    private void Awake()
    {
        buttonPrefab.gameObject.SetActive(false);
    }
    private void Start()
    {
        bool show = snapshotProvider.ShowOnMainMenu && snapshotProvider.Snapshots.Length > 0;
        if (show)
        {
            _onSnapshotsEnabled?.Invoke();
            Populate();
        }
        else
        {
            _onSnapshotsDisabled?.Invoke();
        }
    }
    public void Populate()
    {
        //clear existing buttons
        foreach (SnapshotUIButton child in buttons)
        {
            Destroy(child.gameObject);
        }
        //get snapshots from provider
        foreach (var snapshot in SaveDataSnapshotProvider.Instance.Snapshots)
        {
            var button = Instantiate(buttonPrefab, buttonContainer);
            button.SetData(snapshot, snapshot.Label);
            buttons.Add(button);
            button.transform.SetParent(buttonContainer, false);
            button.gameObject.SetActive(true);
        }
    }
}
