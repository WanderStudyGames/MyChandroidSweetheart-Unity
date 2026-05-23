using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
public class PanelUnfold : MonoBehaviour
{
    [SerializeField] private Vector2 _smallestSize = new(150f, 80f);
    [SerializeField] private float _durationInSeconds = 1;
    [FormerlySerializedAs("_hideObjects")]
    [SerializeField] private GameObject[] _hideObjectsWhileAnimating = new GameObject[] { };
    [SerializeField] private UnityEvent _onOpen;
    [FormerlySerializedAs("_executeAfterClose")]
    [FormerlySerializedAs("_onDisable")]
    [SerializeField] private UnityEvent _onClose;
    [SerializeField] private bool disableOnClose = true;
    [SerializeField] private bool InvokeCloseEventOnDisable = false;
    [SerializeField] private SFX _openSFX;
    [SerializeField] private SFX _closeSFX;


    private RectTransform _rectTransform;
    private Vector2 _defaultSize;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultSize = _rectTransform.sizeDelta;
    }
    public void Close()
    {
        if (!gameObject.activeInHierarchy) return;
        StopAllCoroutines();
        foreach (GameObject go in _hideObjectsWhileAnimating) { go.SetActive(false); }
        if (_closeSFX != null) { _closeSFX.PlayAtPoint(transform.position); }
        StartCoroutine(Co_Size(_rectTransform.sizeDelta, _smallestSize, () =>
        {

            if (disableOnClose)
            {
                gameObject.SetActive(false);
            }
            _onClose.Invoke();
            //if (PlayerStateManager.State == PlayerStates.Menu) PlayerStateManager.SwitchState(PlayerStateManager.PreviousState);
        }));

    }
    private void OnEnable()
    {
        _onOpen.Invoke();
        if (_openSFX != null) { _openSFX.PlayAtPoint(transform.position); }
        StopAllCoroutines();
        foreach (GameObject go in _hideObjectsWhileAnimating) { go.SetActive(false); }
        StartCoroutine(Co_Size(_smallestSize, _defaultSize, () =>
        {
            foreach (GameObject go in _hideObjectsWhileAnimating) { go.SetActive(true); }

        }));
    }
    private void OnDisable()
    {
        if (InvokeCloseEventOnDisable)
            _onClose.Invoke();
        foreach (GameObject go in _hideObjectsWhileAnimating) { go.SetActive(true); }
        _rectTransform.sizeDelta = _defaultSize;
    }
    private IEnumerator Co_Size(Vector2 startSize, Vector2 endSize, Action endAction = null)
    {
        float time = 0;
        _rectTransform.sizeDelta = startSize;
        while (time < _durationInSeconds)
        {
            _rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, time / _durationInSeconds);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        _rectTransform.sizeDelta = endSize;
        endAction?.Invoke();
    }
}
