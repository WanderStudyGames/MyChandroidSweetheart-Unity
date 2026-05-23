using System.Collections;

using UnityEngine;

public class CompanionInteractDetector : MonoBehaviour
{
    [SerializeField] private Companion _companion;
    [SerializeField] private Animator _animator;
    private Collider[] _colliders = new Collider[10];
    private CompanionInteractible target;
    public void SetTarget(CompanionInteractible cInt)
    {
        target = cInt;
        if (target == null) return;
        _colliders = new Collider[_colliders.Length];
        if (Physics.OverlapSphereNonAlloc(transform.position, 2f, _colliders) > 0)
        {
            foreach (Collider other in _colliders)
            {
                if (other != null && other.gameObject == target.gameObject)
                {
                    CompanionManager.RotateTowardsObject(target.transform);
                    Interact(target);
                    break;
                }
            }
        }
    }
    private void Interact(CompanionInteractible inter)
    {
        inter.InteractAction(_companion);
        target = null;
        StopAllCoroutines();
        if (_animator != null && inter.UseArms)
            StartCoroutine(Co_TerminalAnimationLayer());
    }
    IEnumerator Co_TerminalAnimationLayer()
    {
        yield return ExtensionMethods.Co_FadeFloat(0.3f, Vector2.up, fl =>
        {
            _animator.SetLayerWeight(3, fl);
        });
        yield return new WaitForSeconds(2);
        yield return ExtensionMethods.Co_FadeFloat(0.3f, Vector2.right, fl =>
        {
            _animator.SetLayerWeight(3, fl);
        });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            Interact(target);
        }
    }
}
