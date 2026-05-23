using System;
using System.Collections.Generic;

using UnityEngine;

public class CompanionOverrideController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorOverrideController _animatorOverrideController;
    [SerializeField] private LerpToPlayer _lerpToPlayer;
    [SerializeField] private AnimationClip _followWalkAnim;
    [SerializeField] private AnimationClip _followIdleAnim;
    [SerializeField] private AnimationClip _objectWalkAnim;
    [SerializeField] private AnimationClip _objectIdleAnim;
    [SerializeField] private AnimationClip[] _fidgetAnims = new AnimationClip[0];
    [SerializeField] private InteractibleObject _interactibleObject;
    [SerializeField] private bool _unscaledTime;
    [SerializeField] private LookAtSetter[] _lookAtSetters;
    private bool LookingAt() => _reactibles.Count > 0 && _reactibles[0].LookAt;
    private List<CompanionReactible> _reactibles = new();
    private static bool isReacting;
    public static bool IsReacting => isReacting;
    public static event Action OnReactStart;
    public static event Action OnReactEnd;
    private IdleTimer _idleTimer;
    private float transitionDuration = 0.1f;
    void Awake()
    {
        _idleTimer = _animator.GetBehaviour<IdleTimer>();
        _lookAtSetters = _animator.GetBehaviours<LookAtSetter>();
        if (_idleTimer != null)
            _idleTimer._unscaledTime = _unscaledTime;
    }
    public void ResetIdleTimer()
    {
        if (_idleTimer != null)
            _idleTimer.ResetTimer();
    }
    void OnEnable()
    {
        Commands.OnCommand += OnCommand;
        if (!isReacting)
        {
            _animatorOverrideController["Anim_Companion_Walk_F"] = _followWalkAnim;
            _animatorOverrideController["Idle"] = _followIdleAnim;
        }
        if (_idleTimer != null)
            _idleTimer.OnFidget += OnFidget;
        ResetIdleTimer();
    }
    void OnDisable()
    {
        Commands.OnCommand -= OnCommand;
        if (_idleTimer != null)
            _idleTimer.OnFidget -= OnFidget;
    }
    void OnDestroy()
    {
        isReacting = false;
    }
    private void OnFidget()
    {
        if (isReacting) return;
        var clip = _fidgetAnims[UnityEngine.Random.Range(0, _fidgetAnims.Length)];
        _animatorOverrideController["Anim_C_Fidget_Look"] = clip;
        _animator.SetTrigger("Fidget");
    }
    public void ReactTo(CompanionReactible target)
    {
        Debug.Log("Reacting, targeting " + target.name);
        if (_reactibles.Contains(target)) return;
        if (_reactibles.Count == 0) OnReactStart?.Invoke();
        _reactibles.Insert(0, target);
        isReacting = true;
        target.BeforeDisable += UnReact;
        foreach (var setter in _lookAtSetters)
        {
            setter.IsReacting = true;
        }

        if (_interactibleObject != null)
            _interactibleObject.AddEmbargo("Reacting");

        _animatorOverrideController["Anim_C_Afraid"] =
        target.Reaction.Reaction;
        _animatorOverrideController["Anim_C_Shiver"] =
        target.Reaction.Posture;

        StartCoroutine(ExtensionMethods.Co_FadeFloat(transitionDuration, Vector2.up, val =>
        {
            _animator.SetLayerWeight(1, val);
            _animator.SetLayerWeight(4, val);
        }));

        if (target.LookAt)
            _lerpToPlayer.SetTarget(target.transform);
    }
    public void UnReact(CompanionReactible target)
    {
        isReacting = false;
        if (!_reactibles.Contains(target)) return;
        target.BeforeDisable -= UnReact;
        _reactibles.Remove(target);
        bool lookingAt = LookingAt();
        foreach (var setter in _lookAtSetters)
        {
            setter.IsReacting = lookingAt;
        }
        if (_reactibles.Count > 0)
        {
            _lerpToPlayer.SetTarget(_reactibles[0].transform);
            _animatorOverrideController["Anim_C_Afraid"] = _reactibles[0].Reaction.Reaction;
            _animatorOverrideController["Anim_C_Shiver"] = _reactibles[0].Reaction.Posture;
            return;
        }
        //lifecycle end
        StartCoroutine(ExtensionMethods.Co_FadeFloat(transitionDuration, Vector2.right, val =>
        {
            _animator.SetLayerWeight(1, val);
            _animator.SetLayerWeight(4, val);
        }));
        OnReactEnd?.Invoke();
        if (_interactibleObject != null)
            _interactibleObject.RemoveEmbargo("Reacting");
        _lerpToPlayer.TargetPlayer();
        ResetIdleTimer();
    }
    private void OnCommand()
    {
        if (isReacting) return;
        var cmd = CompanionData.CurrentCommand;
        _animator.ClipReplacementCrossFade(transitionDuration, () =>
        {
            if (cmd == null || (cmd.Transform != null && cmd.Transform.tag == PlayerManager.Tag))
            {
                _animatorOverrideController["Anim_Companion_Walk_F"] = _followWalkAnim;
                _animatorOverrideController["Idle"] = _followIdleAnim;
            }
            else if (cmd.Vector3 != default)
            {
                _animatorOverrideController["Anim_Companion_Walk_F"] = null;
                _animatorOverrideController["Idle"] = null;
            }
            else
            {
                _animatorOverrideController["Anim_Companion_Walk_F"] = _objectWalkAnim;
                _animatorOverrideController["Idle"] = _objectIdleAnim;
            }
        }, new string[] { "Idle", "Moving" });
    }
}
