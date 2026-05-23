using System;

using UnityEngine;

public class IdleTimer : StateMachineBehaviour
{
    public event Action OnFidget;
    public bool _unscaledTime;
    private float timer;
    public void ResetTimer()
    {
        timer = UnityEngine.Random.Range(5f, 15f);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = UnityEngine.Random.Range(5f, 15f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= _unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        if (timer <= 0f)
        {
            OnFidget?.Invoke();
            timer = UnityEngine.Random.Range(5f, 15f);
            if (_unscaledTime) Debug.Log("Fidget! (unscaled)");
        }
    }
}
