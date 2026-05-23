using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosplayCrafter : MonoBehaviour
{
    [SerializeField] private Wardrobe _wardrobe;
    [SerializeField] private CurrencyCost _costTemplate;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private SkinnedMeshRenderer _previewParent;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _message;
    [SerializeField] private GameObject _messageGroup;
    [SerializeField] private SFX _selectClothingSFX;
    [SerializeField] private SFX _createClothingSFX;
    private Clothing _selection;
    private SkinnedMeshRenderer _preview;
    private List<CurrencyCost> _costList = new();
    private void Awake()
    {
        _costTemplate.gameObject.SetActive(false);
        _message.text = "";
        _messageGroup.SetActive(false);
        _title.text = "";
    }
    private void OnEnable()
    {
        Select(null);
    }
    public static event Action OnCraftItem;
    public void Select(Clothing clothing)
    {
        foreach (var costDisplay in _costList) { Destroy(costDisplay.gameObject); }
        _costList.Clear();
        if (_preview != null) Destroy(_preview.gameObject);

        _selection = clothing;

        _button.interactable = clothing != null && clothing.Affordable() && !_wardrobe.Clothings.Has(clothing);

        if (clothing == null)
        {
            _messageGroup.gameObject.SetActive(false);
            _title.text = "";
            return;
        }

        _selectClothingSFX?.PlayAtPoint(transform.position);
        _messageGroup.gameObject.SetActive(true);
        _title.text = clothing.Name;

        _preview = clothing.SpawnRenderer(_previewParent);
        _preview.transform.SetParent(_previewParent.transform, false);
        foreach (var cost in clothing.Costs)
        {
            var c = Instantiate(_costTemplate);
            c.transform.SetParent(_costTemplate.transform.parent, false);
            _costList.Add(c);
            c.SetDisplay(cost.Wallet.Icon, cost.Wallet.Color, cost.Amount);
            c.gameObject.SetActive(true);
        }
        _messageGroup.gameObject.SetActive(_costList.Count == 0 || _wardrobe.Clothings.Has(clothing));
        _message.text = (_wardrobe.Clothings.Has(clothing)) ? "OWNED!" : (_costList.Count == 0) ? "FREE!" : "";

    }
    public void CreateClothing()
    {
        if (_selection == null) return;
        _wardrobe.Clothings.Add(_selection);
        foreach (var cost in _selection.Costs)
        {
            cost.Wallet.Add(-cost.Amount);
        }
        Select(_selection);
        OnCraftItem?.Invoke();
        if (_createClothingSFX != null)
        {
            SFX.PlayAtPoint(_createClothingSFX, transform.position);
        }
    }
}
