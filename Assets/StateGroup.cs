using UnityEngine;

public class StateGroup : StateMachineBehaviour
{
    [SerializeField] private int _groupID;
    [SerializeField] private string[] _states;
    private StateGroupManager _sgManager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_sgManager == null) _sgManager = animator.GetComponent<StateGroupManager>();
        _sgManager.AddActiveGroup(_groupID);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _sgManager.RemoveActiveGroup(_groupID);
    }
}
