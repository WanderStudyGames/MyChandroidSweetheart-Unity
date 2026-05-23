using UnityEngine;

[CreateAssetMenu(fileName = "TextureSoundIndex", menuName = "ScriptableObjects/TextureSoundIndex")]
public class TextureSoundIndex : ScriptableObject
{
    [SerializeField] private SFXTextureList[] sfxTextureLists;
}
[System.Serializable]
public class SFXTextureList
{
    [SerializeField] private SFX sfx;
    public SFX SFX => sfx;
    [SerializeField] private Texture2D[] textures;
    public Texture2D[] Textures => textures;

}