using UnityEngine;
[CreateAssetMenu(fileName = "CB_Laser", menuName = "ScriptableObjects/Companion/Behaviors/Laser")]
public class LaserCompanionBehavior : CompanionBehavior
{
    [SerializeField] private NMAProfile _nmaProfile;

    public override NMAProfile NMAProfile => _nmaProfile;
    public override void OnCommand(Transform tr) { }
    public override void OnCommand(Vector3 pos) { }
    public override void Enable()
    {

        Context.SetAnimatorParam.SetBoolTrue("Laser");
        CompanionManager.SetInteractSphereTag("Untagged");
        if (Context.NavMeshAgent.enabled)
            Context.NavMeshAgent.SetDestination(Context.transform.position);
    }
    public override void OnUpdate()
    {
        if (CompanionData.CurrentCommand.Transform != null && CompanionData.CurrentCommand.Transform.CompareTag("Laser"))
        {
            Debug.DrawLine(Context.transform.position, CompanionData.CurrentCommand.Transform.transform.position, Color.magenta);
        }
        else
        {
            Context.SetBehavior(CompanionManager.CompanionBehaviors.IdleBehavior);
        }
    }
    public override void Disable()
    {
        Context.SetAnimatorParam.SetBoolFalse("Laser");
        CompanionManager.SetInteractSphereTag("Dialogue");
    }

    public override void OnInteract() { }

}
