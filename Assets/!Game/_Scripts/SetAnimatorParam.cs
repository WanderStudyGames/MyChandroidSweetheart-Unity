using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SetAnimatorParam : MonoBehaviour
{
    [SerializeField] private string parameterName;
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetBoolFalse()
    {
        animator.SetBool(parameterName, false);
    }
    public void SetBoolFalse(string paramName)
    {
        animator.SetBool(paramName, false);
    }
    public void SetBool(string boolName, bool enabled)
    {
        animator.SetBool(boolName, enabled);
    }
    public void SetBoolTrue()
    {
        animator.SetBool(parameterName, true);
    }
    //public void Play(string stateName, string boolean, float seconds = 0.5f)
    //{
    //    animator.SetBool(boolean, true);
    //    animator.CrossFade(stateName, seconds);
    //}
    public void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    public void SetBoolTrue(string paramName)
    {
        animator.SetBool(paramName, true);
    }
    public void SetFloat(float f)
    {
        animator.SetFloat(parameterName, f);
    }
}
