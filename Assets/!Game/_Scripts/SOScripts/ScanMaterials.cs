using UnityEngine;

[CreateAssetMenu(fileName = "ScanMaterials", menuName = "ScriptableObjects/UI/ScanMaterials")]
public class ScanMaterials : ScriptableObject
{
    public static ScanMaterials Instance;
    private void OnEnable()
    {
        Instance = this;
    }
    public Material unscannedMaterial;
    public Material unscannedMaterial_HL;
    public Material scannedMaterial;
    public Material scannedMaterial_HL;
    public Material importantMaterial;
    public Material importantMaterial_HL;
    public Material rewireMaterial;
    public Material rewireMaterial_HL;
    public Material rewireCable;
    public Material rewireTarget_Inactive;
    public Material rewireTarget;
    public Material rewireTarget_HL;
    public Material rewireMaterial_Clicked;

    public Sprite scanSprite;
    public Sprite rewireSprite;
    public Sprite rewireTargetSprite;
}
