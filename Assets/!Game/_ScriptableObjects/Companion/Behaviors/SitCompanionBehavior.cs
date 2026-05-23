using UnityEngine;

[CreateAssetMenu(fileName = "Sit", menuName = "ScriptableObjects/Companion/Behaviors/Sit")]
public class SitCompanionBehavior : CompanionBehavior
{
    private BehaviorChangeOnExit _behaviorChangeOnExit;
    private bool _exiting;
    private bool _sitting;
    public override bool CanInteract() => false;
    public override void Enable()
    {
        Context.SetAnimatorParam.Animator.SetBool("Sit", true);
        _behaviorChangeOnExit = Context.SetAnimatorParam.Animator.GetBehaviour<BehaviorChangeOnExit>();
        _behaviorChangeOnExit.OnExit += Disable;
        _exiting = false;
        _sitting = true;
        Context.CompanionRigController.SetChestAim(false);
        Context.CompanionRigController.SetLookAt(true);
    }
    public override void Disable()
    {
        if (!_exiting)
        {
            Debug.LogError("EXITING SIT STATE");
            _exiting = true;
            var cmd = CompanionData.CurrentCommand;
            if (cmd == null) Context.SetBehavior(CompanionManager.CompanionBehaviors.FollowBehavior);
            else if (cmd.Transform != null) base.OnCommand(cmd.Transform);
            else base.OnCommand(cmd.Vector3);
        }
        else
        {
            Context.CompanionRigController.SetChestAim(true);
            Context.SetAnimatorParam.Animator.SetBool("Sit", false);
            _behaviorChangeOnExit.OnExit -= Disable;
            _behaviorChangeOnExit = null;
            base.Disable();
        }
    }
    public override void OnCommand(Transform tr)
    {
        OnCommand();
    }
    public override void OnCommand(Vector3 pos)
    {
        OnCommand();
    }
    private void OnCommand()
    {
        if (_sitting)
        {
            Context.SetAnimatorParam.Animator.SetBool("Sit", false);
            _sitting = false;
        }
    }




}
