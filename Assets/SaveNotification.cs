using UnityEngine;
using UnityEngine.Events;

public class SaveNotification : MonoBehaviour
{
    private static SaveNotification _instance;
    [SerializeField] private Animator _notification;
    [SerializeField] private UnityEvent _onShow;
    private void Awake()
    {
        _instance = this;
    }
    public static void Show()
    {
        SaveSystem.Save();
        if (SaveSystem.IsDemo()) return;
        _instance?._onShow.Invoke();
    }
}
