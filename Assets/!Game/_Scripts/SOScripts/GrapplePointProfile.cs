using UnityEngine;

[CreateAssetMenu(fileName = "GrObjPr", menuName = "ScriptableObjects/Scenes/Grapple Object Profile")]
public class GrapplePointProfile : ScriptableObject
{
    [SerializeField] private GameObject particleSystem;
    public GameObject ParticleSystemPrefab => particleSystem;
    [SerializeField] private SFX hookLandSFX;
    public SFX HookLandSFX => hookLandSFX;
}
