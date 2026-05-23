using System.Collections;

using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CompanionRigController : MonoBehaviour
{
    [SerializeField] private Rig _carryRig;
    [SerializeField] private Rig _lookRig;
    [SerializeField] private Rig _kissRig;
    [SerializeField] private MultiAimConstraint _chestAim;
    [SerializeField] private AnimationCurve _lookWeightCurve;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _lerpToPlayer;
    private bool _looking;
    [ContextMenu("SetCarry(true)")]
    private void CarryTrue() { SetCarry(true); }
    [ContextMenu("SetCarry(false)")]
    private void CarryFalse() { SetCarry(false); }
    public void SetCarry(bool b)
    {
        float target = b ? 1 : 0;
        float start = _carryRig.weight;
        if (isActiveAndEnabled)
            StartCoroutine(ExtensionMethods.Co_FadeFloat(0.5f, new(start, target), val =>
            {
                _carryRig.weight = val;
            }));
        else
        {
            _carryRig.weight = target;
        }
    }
    private float AngleToPlayer()
    {
        var toPlayer = _lerpToPlayer.position.XZ() - transform.position.XZ();
        return Vector2.Angle(transform.forward.XZ(), toPlayer);
    }
    private Coroutine _lookAtCoroutine;
    private Coroutine _chestAimCoroutine;
    public void SetLookAt(bool b)
    {
        if (_lookAtCoroutine != null) StopCoroutine(_lookAtCoroutine);
        _looking = false;
        float target = b ? _lookWeightCurve.Evaluate(AngleToPlayer()) : 0;
        float start = _lookRig.weight;
        if (isActiveAndEnabled)
            _lookAtCoroutine = StartCoroutine(Co_SetLookAt());

        IEnumerator Co_SetLookAt()
        {
            yield return ExtensionMethods.Co_FadeFloat(0.5f, new(start, target), val =>
            {
                _lookRig.weight = val;
            });
            _looking = b;
        }
    }
    public void SetChestAim(bool enabled)
    {
        if (_chestAimCoroutine != null) StopCoroutine(_chestAimCoroutine);
        float target = enabled ? 1 : 0;
        float start = _chestAim.weight;
        StartCoroutine(ExtensionMethods.Co_FadeFloat(0.5f, new(start, target), val =>
        {
            _chestAim.weight = val;
        }));
    }
    private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public void InteractRig(InteractRigData data)
    {
        _looking = false;
        StartCoroutine(ExtensionMethods.Co_FadeFloat(data.Timings.x, new(_lookRig.weight, 1), val =>
        {
            _lookRig.weight = val;
        }));
        StartCoroutine(Co_Kiss());
        IEnumerator Co_Kiss()
        {
            yield return ExtensionMethods.Co_FadeFloat(data.Timings.x, new(0, 1), val =>
            {
                foreach (var rig in data.Rigs)
                {
                    rig.weight = _curve.Evaluate(val);
                }
            });
            yield return new WaitForSeconds(data.Timings.y);
            StartCoroutine(ExtensionMethods.Co_FadeFloat(data.Timings.z, new(1, _lookWeightCurve.Evaluate(AngleToPlayer())), val =>
            {
                _lookRig.weight = val;
            }));

            yield return ExtensionMethods.Co_FadeFloat(data.Timings.z, new(1, 0), val =>
            {
                foreach (var rig in data.Rigs)
                {
                    rig.weight = _curve.Evaluate(val);
                }
            });
            _looking = true;
        }
    }
    private LookAtSetter[] _lookAtSetters;
    void Awake()
    {
        _lookAtSetters = _animator.GetBehaviours<LookAtSetter>();
    }
    void OnEnable()
    {
        foreach (LookAtSetter lookAtSetter in _lookAtSetters)
        {
            lookAtSetter.OnLookAtPlayer += SetLookAt;
        }
    }
    void OnDisable()
    {
        foreach (LookAtSetter lookAtSetter in _lookAtSetters)
        {
            lookAtSetter.OnLookAtPlayer -= SetLookAt;
        }
    }

    private void Update()
    {
        //create weight curve based on angle instead

        if (_looking)
        {
            _lookRig.weight = _lookWeightCurve.Evaluate(AngleToPlayer());
        }
    }
}
