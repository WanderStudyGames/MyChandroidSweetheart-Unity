using UnityEngine;

[CreateAssetMenu(fileName = "CommandsProfile", menuName = "ScriptableObjects/Player/Commands Profile")]
[ProfileFor(typeof(Commands))]
public class CommandsProfile : PlayerAbilityProfile
{
    public GameEvent OnCommandGE;
    [Space(30)]
    [SerializeField] private LayerMask _layerMask;
    public LayerMask LayerMask => _layerMask;

    [SerializeField] private GameObject locationIndicatorPF;
    public GameObject LocationIndicatorPF => locationIndicatorPF;

    [SerializeField] private GameObject followIndicatorPF;
    public GameObject FollowIndicatorPF => followIndicatorPF;

    public SFX followSFX;
    public SFX locationSFX;

}