using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "MaterialOverride", menuName = "ScriptableObjects/MaterialOverride")]
public class MaterialOverride : ScriptableObject
{
    public enum ColorModes { None, PlayerColorable, AltTexColorable }
    [field: SerializeField, HideInInspector] public Material Material { get; private set; }
    [field: SerializeField] public Material[] Materials { get; private set; }
    [field: SerializeField] public MaterialOverride[] CopyColorTo { get; private set; }
    [field: SerializeField] public ColorModes ColorMode { get; private set; }
    [field: SerializeField, BeginReadOnlyGroup] public Color OverrideColor { get; private set; }
    public void SetOverrideColor(Color color, bool propogate = true)
    {
        OverrideColor = color;
        if (!propogate || CopyColorTo == null) return;
        foreach (var mo in CopyColorTo) { mo.SetOverrideColor(color, false); }
    }
    [field: SerializeField, EndReadOnlyGroup] public Color DefaultColor { get; private set; }
    [field: SerializeField] public Color DefaultShadowColor { get; private set; }
    [field: SerializeField] public Color DefaultDeepShadowColor { get; private set; }
    [field: SerializeField] public AlternateTexture[] AlternateTextures { get; private set; } = { };
    public int TextureIndex { get; set; }
    public void ApplyOverrides()
    {
        foreach (var material in Materials)
        {
            if (ColorMode == ColorModes.PlayerColorable && OverrideColor != DefaultColor)
            {
                material.SetToonColor(OverrideColor);
                if (material.HasColor("_Color1"))
                    material.SetColor("_Color1", OverrideColor);
            }
            else if (ColorMode == ColorModes.PlayerColorable)
            {
                material.SetToonColors(DefaultColor, DefaultShadowColor, DefaultDeepShadowColor);
                if (material.HasColor("_Color1"))
                    material.SetColor("_Color1", DefaultColor);
            }
            if (AlternateTextures.Length > 0 && TextureIndex > -1 && TextureIndex < AlternateTextures.Length)
            {
                material.SetTexture("_MainTex", AlternateTextures[TextureIndex].Texture);
                if (material.HasTexture("_Emissive_Tex"))
                    material.SetTexture("_Emissive_Tex", AlternateTextures[TextureIndex].Texture);
                if (ColorMode == ColorModes.AltTexColorable)
                    material.SetToonColors(AlternateTextures[TextureIndex].Color, AlternateTextures[TextureIndex].ShadowColor, AlternateTextures[TextureIndex].DeepShadowColor);
            }
        }
    }
    private void OnEnable()
    {
        SaveSystem.OnLoadFile += Load;
        SaveSystem.OnSaveFile += Save;
    }
    private void OnDisable()
    {
        SaveSystem.OnLoadFile -= Load;
        SaveSystem.OnSaveFile -= Save;
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    private void Load(SaveSystem.SaveFileNames files)
    {
        SetOverrideColor(ES3.Load(name + "_Color", files.materialData, DefaultColor));
        TextureIndex = ES3.Load(name + "_Texture", files.materialData, 0);
        ApplyOverrides();//
    }
    private void Save(SaveSystem.SaveFileNames files)
    {
        if (ColorMode == ColorModes.PlayerColorable)
            ES3.Save(name + "_Color", OverrideColor, files.materialData);
        if (AlternateTextures.Length > 0)
            ES3.Save(name + "_Texture", TextureIndex, files.materialData);
    }
    [Button]
    public void ResetOverrides()
    {
        OverrideColor = DefaultColor;
        foreach (var material in Materials)
        {
            if (ColorMode != ColorModes.None)
            {
                material.SetToonColors(DefaultColor, DefaultShadowColor, DefaultDeepShadowColor);
                if (material.HasColor("_Color1"))
                    material.SetColor("_Color1", DefaultColor);
            }
            if (AlternateTextures.Length > 0)
            {
                material.SetTexture("_MainTex", AlternateTextures[0].Texture);
                if (material.HasTexture("_Emissive_Tex"))
                    material.SetTexture("_Emissive_Tex", AlternateTextures[0].Texture);
            }
        }

        TextureIndex = 0;
    }
    [System.Serializable]
    public class AlternateTexture
    {
        [field: SerializeField] public Texture2D Texture { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public Color ShadowColor { get; private set; }
        [field: SerializeField] public Color DeepShadowColor { get; private set; }
    }
}