using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class LiftDestination : MonoBehaviour
{
    [SerializeField, Dependency] private Lift _lift;
    [SerializeField] private NavMeshObstacle _obstacle;
    [SerializeField] private int _destinationIndex;
    [SerializeField] private UnityEvent _onLiftStart;
    [SerializeField] private UnityEvent _onLiftArrive;
#if UNITY_EDITOR
    [SerializeField] public MeshFilter MeshFilter { get; private set; }
#endif
    private void Awake()
    {
        if (_obstacle != null && _destinationIndex != 0)
            _obstacle.enabled = true;
    }
    private void OnEnable()
    {
        _lift.OnFinish += LiftFinish;
        _lift.OnStart += LiftStart;
    }
    private void OnDisable()
    {
        _lift.OnFinish -= LiftFinish;
        _lift.OnStart -= LiftStart;
    }
    public void LiftStart(int i)
    {
        _onLiftStart.Invoke();
        if (_obstacle == null) return;
        _obstacle.enabled = true;
    }
    public void LiftFinish(int i)
    {
        if (i == _destinationIndex)
        {
            _onLiftArrive.Invoke();
            if (_obstacle != null)
                _obstacle.enabled = false;
        }
    }

}
