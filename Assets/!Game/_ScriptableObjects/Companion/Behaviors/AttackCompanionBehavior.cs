using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Companion/Behaviors/Attack")]
public class AttackCompanionBehavior : CompanionBehavior
{
    [SerializeField] private NMAProfile _nmaProfile;

    public override NMAProfile NMAProfile => _nmaProfile;

    public override void Enable()
    {
        if (Context.Companion.CarriedObject != null && CompanionData.CurrentCommand.Transform == null)
        {
            Context.Companion.DropCarriedItem();
        }
        Context.CompanionRigController.SetLookAt(false);
    }

    public override void OnUpdate()
    {
        var companion = Context.Companion;
        if (!Context.NavMeshAgent.isActiveAndEnabled || !Context.NavMeshAgent.isOnNavMesh) return;
        if (companion.AttackTargets.Count > 0)
        {
            Context.NavMeshAgent.SetClosestReachableDestination(companion.AttackTargets[0].Position);
            Context.NavMeshAgent.isStopped = false;
            if (Vector3.Distance(CompanionData.Position, Context.Companion.AttackTargets[0].Position) < 2f)
            {
                ICompanionAttackable attackable = companion.AttackTargets[0];
                companion.RemoveAttackTarget(companion.AttackTargets[0]);
                attackable.Damage();
            }
        }
        else
        {
            CompanionManager.StandInPlace();
            Context.SetBehavior(CompanionManager.CompanionBehaviors.IdleBehavior);
        }
    }

}
