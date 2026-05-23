using System.Collections.Generic;
using UnityEngine;

public class ClothingRemovalButtonManager : MonoBehaviour
{
    [SerializeField] private Outfit _outfit;
    [SerializeField] private ClothingRemover _removerTemplate;
    private List<ClothingRemover> _removers = new();
    void Start()
    {
        UpdateDisplay();
    }
    private void Awake()
    {
        _outfit.Clothings.OnDataModified += UpdateDisplay;
    }
    private void OnDestroy()
    {
        _outfit.Clothings.OnDataModified -= UpdateDisplay;
    }
    private void UpdateDisplay()
    {
        for (int i = _removers.Count - 1; i >= 0; i--)
        {
            ClothingRemover remover = _removers[i];
            Destroy(remover.gameObject);
            _removers.Remove(remover);
        }
        foreach (Clothing clothing in _outfit.Clothings.GetValues())
        {
            var remover = Instantiate(_removerTemplate);
            remover.transform.SetParent(transform, false);
            remover.Clothing = clothing;
            remover.Outfit = _outfit;
            remover.gameObject.SetActive(true);
            _removers.Add(remover);
        }
    }
}
