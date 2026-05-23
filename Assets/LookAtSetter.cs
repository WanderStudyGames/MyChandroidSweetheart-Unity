using System;

using UnityEngine;

public class LookAtSetter : StateMachineBehaviour
{
    [SerializeField] private bool lookAtPlayer;
    [SerializeField] private bool isMovingState;
    public event Action<bool> OnLookAtPlayer;
    public bool IsReacting { get; set; }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //whether or not the companion should be looking at the player or an object is very complicated. 
        // I'd prefer not to handle that logic here, but if I don't, 
        // the Idle state (which bizarrely reinitializes while moving) will set looking at to be fully enabled every time.


        //I need this current logic to function as is, but with the addition that if the companion is afraid, looking at will be enabled.
        if (isMovingState)
        {
            OnLookAtPlayer?.Invoke(IsReacting || (CompanionData.CurrentCommand != null && CompanionData.CurrentCommand.Transform != null && CompanionData.CurrentCommand.Transform.CompareTag(PlayerManager.Tag)));
        }
        else
            OnLookAtPlayer?.Invoke(lookAtPlayer);
    }
}
