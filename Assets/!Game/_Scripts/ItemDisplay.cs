using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : Selectable
{
    [field: SerializeField] public InventoryItemMetadata _itemMetadata;
    // Start is called before the first frame update
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [field: SerializeField] public TMP_Text _description;
    [SerializeField] private UnityEvent _onSelect;
    [SerializeField] private UnityEvent _onDeselect;
    protected override void Start()
    {
        base.Start();
        Populate();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        Populate();
    }
#endif
    public event Action OnItemSelect;
    public event Action OnItemDeselect;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        PopulateDescription();
        _onSelect.Invoke();
        OnItemSelect?.Invoke();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PopulateDescription();
        _onSelect.Invoke();
        OnItemDeselect?.Invoke();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        ClearDescription();
        _onDeselect.Invoke();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        ClearDescription();
        _onDeselect.Invoke();
    }
    public void Populate()
    {
        if (_itemMetadata == null) return;
        if (_icon != null)
            _icon.sprite = _itemMetadata.Icon;
        if (_name != null)
            _name.text = _itemMetadata.Name;
    }
    public void PopulateDescription()
    {
        if (_itemMetadata == null) return;
        if (_description != null)
        {
            _description.text = StringFunctions.ProcessGameText(_itemMetadata.Description);
            //_description.transform.parent.gameObject.SetActive(true);
        }
    }
    public void ClearDescription()
    {
        _description.text = "";
        //_description.transform.parent.gameObject.SetActive(false);
    }
    public void Clear()
    {
        _icon.sprite = null;
        _name.text = "";
        _description.text = "";
    }
}
