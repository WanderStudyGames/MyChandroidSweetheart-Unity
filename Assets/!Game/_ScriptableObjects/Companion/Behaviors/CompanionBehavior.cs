using System.Linq;

using UnityEngine;
using UnityEngine.AI;

public abstract class CompanionBehavior : ScriptableObject
{
    public CompanionBehaviorManager Context { get; set; }
    public virtual NMAProfile NMAProfile => null;
    public virtual void OnUpdate() { }

    public virtual void OnCommand(Transform tr)
    {
        if (tr.gameObject.CompareTag(PlayerManager.Tag))
        {
            Context.SetBehavior(CompanionManager.CompanionBehaviors.FollowBehavior);
            Context.Companion.DoFollowIndicator();
        }
        //else if (go.tag == "Laser")
        //{
        //    //start laser
        //    Debug.Log("Laserrrr");
        //    //switch to laser behavior
        //    Context.SetBehavior(CompanionManager.CompanionBehaviors.LaserBehavior);
        //}
        else
        {
            Context.SetBehavior(CompanionManager.CompanionBehaviors.IdleBehavior);
            if (Context.NavMeshAgent.NavMeshExists())
            {
                Context.CompanionRigController.SetLookAt(false);
                if (Context.NavMeshAgent.enabled)
                {
                    Context.NavMeshAgent.SetClosestReachableDestination(tr.position);
                    Context.NavMeshAgent.isStopped = false;
                }
                if (tr.TryGetComponent(out CompanionInteractible ci))
                {
                    Context.Companion.CompanionInteractDetector.SetTarget(ci);
                }
            }
        }
    }
    public virtual void OnCommand(Vector3 pos)
    {
        if (!Context.NavMeshAgent.isActiveAndEnabled || pos == Vector3.zero) return;
        if (!Context.NavMeshAgent.enabled) return;
        if (!Context.NavMeshAgent.isOnNavMesh) return;
        Context.CompanionRigController.SetLookAt(false);
        Context.NavMeshAgent.SetDestination(pos);
        var path = new NavMeshPath();
        Context.NavMeshAgent.CalculatePath(pos, path);
        NavMesh.SamplePosition(pos, out NavMeshHit hit, 10000, NavMesh.AllAreas);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            CompanionData.Command(hit.position);
            Context.NavMeshAgent.SetClosestReachableDestination(hit.position);
        }
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            var last = path.corners.Last();
            CompanionData.Command(last);
            Context.NavMeshAgent.SetDestination(last);
        }
        else
        {
            Context.NavMeshAgent.SetDestination(pos);
        }
        //Context.NavMeshAgent.SetDestinationNoPartial(pos);
        Context.NavMeshAgent.isStopped = false;
        Context.SetBehavior(CompanionManager.CompanionBehaviors.IdleBehavior);
    }
    public virtual void Enable() { }
    public virtual void Disable() { }
    public virtual bool CanInteract() => Context.Companion.CarriedObject != null;
    public virtual void OnInteract()
    {
        if (Context.Companion.CarriedObject != null)
        {
            Context.Companion.DropCarriedItem();
        }
        else
        {
            CompanionManager.ExecuteInteract();
        }
        //dialogue tree
    }
    public virtual void OnTriggerStay(Collider other)
    {
    }
}
