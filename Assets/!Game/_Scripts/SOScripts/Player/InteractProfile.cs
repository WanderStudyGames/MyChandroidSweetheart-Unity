using UnityEngine;

[CreateAssetMenu(fileName = "Interact Profile", menuName = "ScriptableObjects/Player/Interact Profile")]
[ProfileFor(typeof(Interact))]
public class InteractProfile : PlayerComponentProfile
{
    [Range(0, 10)]
    [SerializeField] private float maxDistance = 3;
    public float MaxDistance => maxDistance;
    [SerializeField] private LayerMask interactLayerMask;
    public LayerMask InteractLayerMask => interactLayerMask;

    [SerializeField] private SFX emptySFX;
    [SerializeField] private SFX failureSFX;
    [field: SerializeField] public Sprite FailureSprite { get; private set; }
    public SFX EmptySFX => emptySFX;
    public SFX FailureSFX => failureSFX;
}