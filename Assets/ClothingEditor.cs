using System.Collections.Generic;
using UnityEngine;

public class ClothingEditor : MonoBehaviour
{
    [SerializeField] private ColorableMaterialEditor _materialEditorTemplate;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private Outfit _outfit;

    private Clothing _selectedClothing;
    private List<ColorableMaterialEditor> _materialEditors = new();
    private void Awake()
    {
        _materialEditorTemplate.gameObject.SetActive(false);
    }
    public void Select(Clothing clothing)
    {
        //_colorPicker.color = _selectedClothing.ColorableMaterials[0].OverrideColor;

        _selectedClothing = clothing;
        GenerateMaterialEditors();
        gameObject.SetActive(true);
    }
    private void GenerateMaterialEditors()
    {
        PurgeEditors();
        foreach (var mo in _selectedClothing.MaterialOverrides)
        {
            var cme = Instantiate(_materialEditorTemplate, _contentTransform);
            cme.SetMaterialOverride(mo);
            cme.gameObject.SetActive(true);
            _materialEditors.Add(cme);
        }
    }
    private void PurgeEditors()
    {
        foreach (var editor in _materialEditors) { Destroy(editor.gameObject); }
        _materialEditors.Clear();
    }
    public void Deselect()
    {
        PurgeEditors();
        _selectedClothing = null;
        gameObject.SetActive(false);
    }
    public void RemoveClothing()
    {
        if (_selectedClothing == null) return;
        _outfit.Clothings.Remove(_selectedClothing);
        Deselect();
    }
}
