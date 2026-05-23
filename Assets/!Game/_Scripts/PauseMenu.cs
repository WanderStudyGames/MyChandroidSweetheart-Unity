using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseAction;
    [SerializeField] private bool _freezeTime = true;
    [SerializeField] private UnityEvent _onPauseUE;
    [SerializeField] private UnityEvent _onUnPauseUE;
    public static bool DisablePause;
    public static event Action OnOpen;
    public static event Action OnClose;
    public static PauseMenu _openMenu;
    private bool _isPaused;
    public bool IsPaused => _isPaused;


    private void OnEnable()
    {
        Debug.Log($"{_pauseAction == null}, {this.name}", this.gameObject);
        if (_pauseAction == null) return;
        _pauseAction.action.Link(OnPauseAction);
        _pauseAction.action.Enable();
    }
    private void OnDisable()
    {
        if (_openMenu == this)
            _openMenu = null;
        if (_pauseAction != null)
            _pauseAction.action.UnLink(OnPauseAction);

    }
    private void OnPauseAction(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame() && _openMenu == null)
        {
            Pause();
        }
    }
    public void Pause()
    {
        if (DisablePause || _openMenu != null) return;
        _openMenu = this;
        _isPaused = true;
        GlobalPlayerInput.DisableComponentInstance();
        StartCoroutine(Co_Pause());
        IEnumerator Co_Pause()
        {
            yield return null;
            if (_freezeTime)
                Time.timeScale = 0;
            OnOpen?.Invoke();
            _onPauseUE.Invoke();
        }
    }
    private void OnDestroy()
    {
        enabled = false;
    }
    public void UnPause()
    {
        if (_openMenu != this) return;
        _openMenu = null;
        _isPaused = false;
        GlobalPlayerInput.EnableComponentInstance();
        if (GlobalPlayerInput.Instance != null)
            GlobalPlayerInput.Instance.StartCoroutine(Co_UnPause());
        else if (gameObject.activeInHierarchy && !gameObject.IsDestroyed()) StartCoroutine(Co_UnPause());
        IEnumerator Co_UnPause()
        {
            Time.timeScale = 1;
            yield return null;
            OnClose?.Invoke();
            yield return null;
            _onUnPauseUE.Invoke();

        }
    }

}
