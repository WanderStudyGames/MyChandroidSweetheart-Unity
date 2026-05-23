using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Button _button;
    [SerializeField] private UnityEvent _onNormal;
    [SerializeField] private UnityEvent _onHighlighted;
    [SerializeField] private UnityEvent _onPressed;
    [SerializeField] private UnityEvent _onReleased;
    [SerializeField] private SFX _hoverSfx;
    [SerializeField] private SFX _unHoverSfx;
    [SerializeField] private SFX _pressSfx;
    [SerializeField] private SFX _releaseSfx;
    public void OnPointerEnter(PointerEventData _)
    {
        if (_button != null && !_button.interactable) return;
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("Highlighted");
            buttonAnimator.ResetTrigger("Normal");
            buttonAnimator.ResetTrigger("Pressed");
        }
        _onHighlighted.Invoke();
        if (_hoverSfx != null)
        {
            _hoverSfx.PlayAtPoint(transform.position);
        }
    }

    public void OnPointerExit(PointerEventData _)
    {
        if (buttonAnimator != null)
            buttonAnimator?.SetTrigger("Normal");
        _onNormal.Invoke();
        if (_unHoverSfx != null)
        {
            _unHoverSfx.PlayAtPoint(transform.position);
        }
    }

    public void OnPointerDown(PointerEventData _)
    {
        if (_button != null && !_button.interactable) return;
        if (buttonAnimator != null)
            buttonAnimator?.SetTrigger("Pressed");
        _onPressed.Invoke();
        if (_pressSfx != null)
        {
            _pressSfx.PlayAtPoint(transform.position);
        }
    }
    public void OnPointerUp(PointerEventData _)
    {
        if (_button != null && !_button.interactable) return;
        if (buttonAnimator != null)
            buttonAnimator?.SetTrigger("Normal");
        //do not invoke _onReleased if cursor simply exits the button
        if (_ != null && _.IsPointerMoving()) return;
        _onReleased.Invoke();
        if (_releaseSfx != null)
        {
            _releaseSfx.PlayAtPoint(transform.position);
        }
    }
    public void OnPointerExit()
    {
        OnPointerExit(null);
    }
    public void OnPointerEnter()
    {
        OnPointerEnter(null);
    }
    public void OnPointerDown()
    {
        OnPointerDown(null);
    }

    public void OnPointerUp()
    {
        OnPointerUp(null);
    }

}