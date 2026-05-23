using System.Collections;
using TMPro;
using UnityEngine;

public class ColorableMaterialEditor : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private TMP_Text _variantLabel;
    [SerializeField] private FlexibleColorPicker _colorPicker;
    [SerializeField] private GameObject _colorPickerGroup;
    [SerializeField] private GameObject _colorSwatchesGroup;
    [SerializeField] private GameObject _dropdownParent;
    [SerializeField] private GameObject _colorParent;
    [SerializeField] private GameObject _resetButton;
    [SerializeField] private MaterialOverride _materialOverride;
    bool _started = false;
    private void Awake()
    {
        if (_materialOverride == null) return;
        //Debug.Log(_materialOverride.OverrideColor);
        _dropdown.options = new();
        foreach (var at in _materialOverride.AlternateTextures)
        {
            _dropdown.options.Add(new(at.Name));
        }
        _dropdown.value = _materialOverride.TextureIndex;
        UpdateDisplay();
    }
    public void SetPickerEnabled(bool enabled)
    {
        _started = false;
        _colorPickerGroup.SetActive(enabled);
        _colorSwatchesGroup.SetActive(!enabled);
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        _started = false;
        StartCoroutine(Co_UpdateDisplay());
        IEnumerator Co_UpdateDisplay()
        {

            if (_materialOverride == null) yield break;
            _colorPicker.color = _materialOverride.OverrideColor;
            _colorParent.gameObject.SetActive(_materialOverride.ColorMode == MaterialOverride.ColorModes.PlayerColorable);
            _dropdownParent.SetActive(_materialOverride.AlternateTextures.Length > 1);
            _dropdown.value = _materialOverride.TextureIndex;
            if (_materialOverride.TextureIndex < _materialOverride.AlternateTextures.Length && _materialOverride.TextureIndex > -1)
                _variantLabel.text = _materialOverride.AlternateTextures[_materialOverride.TextureIndex].Name;
            _resetButton.SetActive(_materialOverride.ColorMode != MaterialOverride.ColorModes.None || _materialOverride.AlternateTextures.Length > 0);

            yield return null;
            _started = true;
        }
    }
    private void OnEnable()
    {
        _colorParent.SetActive(true);
        UpdateDisplay();
    }

    public void SetMaterialOverride(MaterialOverride matOverride) { _materialOverride = matOverride; }
    private void OnDisable()
    {
        if (_materialOverride != null)
            _colorPicker.color = _materialOverride.OverrideColor;

        _colorParent.SetActive(false);
        _started = false;
    }
    public void SetColor(Color color)
    {
        if (_materialOverride == null || !_started || !gameObject.activeInHierarchy) return;
        _materialOverride.SetOverrideColor(color);
        if (_materialOverride.name == "MaterialOverride_Hair")
            Debug.Log(color);
        _materialOverride.ApplyOverrides();
    }
    public void SetPickerColor(Color color)
    {
        _colorPicker.color = color;
    }
    public void SetTextureIndex(int index)
    {
        _materialOverride.TextureIndex = index;
        Debug.Log(_materialOverride.TextureIndex);
        _materialOverride.ApplyOverrides();
        Debug.Log(_materialOverride.TextureIndex);
        UpdateDisplay();
    }
    public void ResetOverrides()
    {
        Debug.Log("RESETTING COLOR");
        _started = false;
        _materialOverride.ResetOverrides();
        UpdateDisplay();
    }
}