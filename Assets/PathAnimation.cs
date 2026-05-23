using PathCreation;
using UnityEngine;
using UnityEngine.Events;

public class PathAnimation : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _duration = 1;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private UnityEvent _onFinish;

    public PathCreator PathCreator => _pathCreator;
    public float Duration => _duration;
    public AnimationCurve AnimationCurve => _animationCurve;
    public UnityEvent OnFinish => _onFinish;

}
