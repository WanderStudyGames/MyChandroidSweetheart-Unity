using System.Collections;

using UnityEngine;

public class CShortReaction : MonoBehaviour
{
    [SerializeField] private AnimationClip[] _reactionAnims;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorOverrideController _aoc;
    [SerializeField] private CompanionOverrideController _companionOverrideController;
    void OnEnable()
    {
        CompanionOverrideController.OnReactStart += StopAllCoroutines;
    }
    void OnDisable()
    {
        CompanionOverrideController.OnReactStart -= StopAllCoroutines;
    }
    public void React()
    {
        _aoc["Anim_C_Afraid"] = _reactionAnims[Random.Range(0, _reactionAnims.Length)];
        StartCoroutine(Co_React());
        IEnumerator Co_React()
        {
            yield return ExtensionMethods.Co_FadeFloat(0.3f, Vector2.up, val =>
            {
                _animator.SetLayerWeight(1, val);
            });
            yield return new WaitForSeconds(0.3f);
            yield return ExtensionMethods.Co_FadeFloat(0.3f, Vector2.right, val =>
            {
                _animator.SetLayerWeight(1, val);
            });
        }
    }
}
