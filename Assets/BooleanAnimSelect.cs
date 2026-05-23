using UnityEngine;

public class BooleanAnimSelect : StateMachineBehaviour
{
    [SerializeField] private string _boolName;
    [SerializeField] private string _trueStateName;
    [SerializeField] private string _falseStateName;
    [SerializeField] private float _transitionDuration = 0.25f;
    bool inState;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)

    {
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (inState) return;
        Debug.LogError("entering boolean anim select state");
        inState = true;
        if (animator.GetBool(_boolName))
            animator.CrossFade(_trueStateName, _transitionDuration);
        else
        {
            animator.CrossFade(_falseStateName, _transitionDuration);
            Debug.LogError("fuckass bitch ass");
        }
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        inState = false;
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
