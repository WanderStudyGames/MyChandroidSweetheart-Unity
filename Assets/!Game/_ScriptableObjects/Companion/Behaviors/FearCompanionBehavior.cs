using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "CB_Fear", menuName = "ScriptableObjects/Companion/Behaviors/Fear")]
public class FearCompanionBehavior : CompanionBehavior
{
    [SerializeField] private NMAProfile _nmaProfile;
    [SerializeField] private SFX _screamSFX;
    [SerializeField] private SFX _scaredLoopSFX;
    [SerializeField] private SFX _relievedSFX;
    [SerializeField] private DialogueData _stillScaredDialogue;

    private AudioSource _loopSource;

    public override NMAProfile NMAProfile => _nmaProfile;
    public override void OnCommand(Transform tr) { }
    public override void OnCommand(Vector3 pos) { }
    public override void Enable()
    {
        if (Context.Companion.CarriedObject != null)
        {
            Context.Companion.DropCarriedItem();
        }
        Context.IgnoreInteractEmbargos(true);
        Context.NavMeshAgent.isStopped = false;
        Context.SetAnimatorParam.Animator.Play("Idle");
        Context.SetAnimatorParam.SetBoolTrue("Fear");
        Debug.Log("wtf");
        SFX.PlayAtPoint(_screamSFX, Context.transform.position);
        if (_scaredLoopSFX != null)
        {
            Context.StartCoroutine(Co_LoopSFX());
            IEnumerator Co_LoopSFX()
            {
                yield return new WaitForSeconds(0.8f);
                _loopSource = SFX.PlayAtPoint(_scaredLoopSFX, Context.transform.position);
            }
        }
        CompanionManager.SetInteractIcon(null);
        Context.NavMeshAgent.SetDestination(Context.transform.position);
    }
    public override void Disable()
    {
        Context.SetAnimatorParam.SetBoolFalse("Fear");
        Context.IgnoreInteractEmbargos(false);
        CompanionManager.SetInteractIcon(Context.Companion.DialogueSprite);
        _loopSource?.Stop();
        _loopSource = null;
        if (_relievedSFX != null)
            SFX.PlayAtPoint(_relievedSFX, Context.transform.position);
    }
    public override bool CanInteract() => true;

    public override void OnInteract()
    {
        var colliders = Physics.OverlapSphere(Context.transform.position, 2f, -1, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Fear")
            {
                //DialogueUI.Speak(_stillScaredDialogue);
                Debug.Log("yeaaa");
                return;
            }
        }
        Context.SetBehavior(CompanionManager.CompanionBehaviors.FollowBehavior);
    }

}
