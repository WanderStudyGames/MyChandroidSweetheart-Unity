using UnityEngine;

[CreateAssetMenu(fileName = "Follow", menuName = "ScriptableObjects/Companion/Behaviors/Follow")]
public class FollowCompanionBehavior : CompanionBehavior
{
    [SerializeField] private NMAProfile _nmaProfile;
    [SerializeField] private ExecuteUnityEvent _interactObject;

    public override bool CanInteract() => true;

    public override NMAProfile NMAProfile => _nmaProfile;
    public override void Enable()
    {
        Context.CompanionRigController.SetLookAt(true);
        if (CompanionData.CurrentCommand == null || CompanionData.CurrentCommand.Transform == null) PlayerManager.CommandFollow();
    }
    public override void OnUpdate()
    {
        var companion = Context.Companion;
        var navMeshAgent = Context.NavMeshAgent;
        if (!navMeshAgent.isActiveAndEnabled || !navMeshAgent.isOnNavMesh) return;
        navMeshAgent.SetClosestReachableDestination(PlayerData.Position);
        float remainingDistance = Vector3.Distance(navMeshAgent.gameObject.transform.position, navMeshAgent.destination);
        if (remainingDistance < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;

            if (companion.AttackTargets.Count > 0) { Context.SetBehavior(CompanionManager.CompanionBehaviors.AttackBehavior); }
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
}
