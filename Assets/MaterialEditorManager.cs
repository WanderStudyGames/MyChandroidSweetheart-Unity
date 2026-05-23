using System.Collections.Generic;
using UnityEngine;

public class MaterialEditorManager : MonoBehaviour
{
    [SerializeField] private Clothing _clothing;
    [SerializeField] private ColorableMaterialEditor _materialEditorTemplate;
    [SerializeField] private RectTransform _materialEditorParent;
    private List<ColorableMaterialEditor> _materialEditors = new();
    private void Awake()
    {
        _materialEditorTemplate.gameObject.SetActive(false);
        RefreshDisplay();
    }
    public void SetClothing(Clothing clothing) { _clothing = clothing; RefreshDisplay(); }
    private void RefreshDisplay()
    {
        foreach (var me in _materialEditors) { Destroy(me.gameObject); }
        _materialEditors.Clear();
        if (_clothing == null) return;
        foreach (var mo in _clothing.MaterialOverrides)
        {
            if (mo.ColorMode == MaterialOverride.ColorModes.None && mo.AlternateTextures.Length == 0) continue;
            var cme = Instantiate(_materialEditorTemplate, _materialEditorParent);
            cme.SetMaterialOverride(mo);
            cme.gameObject.SetActive(true);
            _materialEditors.Add(cme);
        }
    }
}
