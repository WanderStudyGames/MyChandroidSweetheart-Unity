using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtension : Button
{
    [SerializeField] private UnityEvent _onPointerEnter;
    [SerializeField] private UnityEvent _onPointerExit;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onDeselect;
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
    }
}
