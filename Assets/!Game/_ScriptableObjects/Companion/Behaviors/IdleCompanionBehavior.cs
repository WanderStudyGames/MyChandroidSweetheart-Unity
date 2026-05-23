using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Idle", menuName = "ScriptableObjects/Companion/Behaviors/Idle")]
public class IdleCompanionBehavior : CompanionBehavior
{
    [SerializeField] private NMAProfile _nmaProfile;
    [SerializeField] private AnimationClip _normalWalkAnim;
    [SerializeField] private AnimationClip _normalIdleAnim;
    [SerializeField] private AnimationClip _objectWalkAnim;
    [SerializeField] private AnimationClip _objectIdleAnim;
    [SerializeField] private AnimatorOverrideController _animatorOverrideController;
    public override NMAProfile NMAProfile => _nmaProfile;
    public override bool CanInteract() => true;
    public static Transform RotationTarget = null;
    private Companion companion;
    private NavMeshAgent nma;
    private Command cmd;
    public override void Enable()
    {
        base.Enable();
        Context.CompanionRigController.SetLookAt(false);
    }
    public override void OnUpdate()
    {

        companion = Context.Companion;
        nma = Context.NavMeshAgent;
        cmd = CompanionData.CurrentCommand;
        if (!nma.isActiveAndEnabled || !nma.isOnNavMesh) return;


        if (cmd == null) return;

        Vector3 pos = cmd.Vector3 != Vector3.zero ? cmd.Vector3 : cmd.Transform != null ? cmd.Transform.position : nma.destination;

        if (cmd.Vector3 != Vector3.zero)
        {
            nma.SetDestination(cmd.Vector3);
        }
        else if (cmd.Transform != null)
        {
            nma.SetClosestReachableDestination(cmd.Transform.position);
        }

        float remainingDistance = Vector3.Distance(nma.gameObject.transform.position, pos);

        if (remainingDistance <= nma.stoppingDistance)
        {
            nma.isStopped = true;
            if (RotationTarget != null)
            {
                nma.transform.rotation = Quaternion.Euler(
                    nma.transform.rotation.eulerAngles.x,
                    RotationTarget.rotation.eulerAngles.y,
                    nma.transform.rotation.eulerAngles.z
                    );
                RotationTarget = null;
            }
            Context.CompanionRigController.SetLookAt(true);
            if (companion.AttackTargets.Count > 0)
            {
                Context.SetBehavior(CompanionManager.CompanionBehaviors.AttackBehavior);
            }
            CompanionData.Command();
        }
    }
}
