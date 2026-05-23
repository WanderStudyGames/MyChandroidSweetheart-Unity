using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Outfit", menuName = "ScriptableObjects/Clothings/Outfit")]
public class Outfit : ScriptableObject
{
    public SavedScriptableObjectList<Clothing> Clothings { get; private set; } = null;
    public List<Clothing> _defaultClothings;
    private void OnEnable()
    {
        Clothings = new($"{name}_clothings", files => files.companionData, _defaultClothings);
    }
    public SkinnedMeshRenderer[] SpawnClothings(SkinnedMeshRenderer skinnedMeshRenderer, bool noClothes = false)
    {
        var clothings = Clothings.GetValues();
        List<SkinnedMeshRenderer> renderers = new();
        for (int i = clothings.Length - 1; i >= 0; i--)
        {
            var clothing = clothings[i];
            if (clothing == null)
            {
                Clothings.Remove(clothing);
                continue;
            }
            if (noClothes && (clothing.Type == Clothing.ClothingType.Top || clothing.Type == Clothing.ClothingType.Bottom)) continue;
            var smr = clothing.SpawnRenderer(skinnedMeshRenderer);
            Debug.Assert(smr != null);
            smr.updateWhenOffscreen = true;
            smr.gameObject.layer = skinnedMeshRenderer.gameObject.layer;
            smr.transform.SetParent(skinnedMeshRenderer.transform, false);
            smr.transform.ResetLocal();
            renderers.Add(smr);
        }
        return renderers.ToArray();
    }
    public void RemoveAllClothingOfType(Clothing.ClothingType type)
    {
        var clothings = Clothings.GetValues();
        for (int i = clothings.Length - 1; i >= 0; i--)
        {
            if (clothings[i].Type == type)
            {
                Clothings.Remove(clothings[i]);
            }
        }
    }

}