using UnityEngine;

public class GiveAbility : MonoBehaviour
{
    [SerializeField] private PlayerAbilityProfile pa;
    public void SetAbility()
    {
        if (pa == null) { return; }
        PlayerAbilityManager.GiveAbilityItem(pa);
    }
}
