using UnityEngine;
using UnityEngine.Events;

public class SprocketPushable : MonoBehaviour, ISprocketPushable
{
    [SerializeField][Range(0, 1)] private float _forceThreshhold = 0f;
    [SerializeField] private UnityEvent _positiveX;
    [SerializeField] private UnityEvent _negativeX;
    [SerializeField] private UnityEvent _positiveY;
    [SerializeField] private UnityEvent _negativeY;
    [SerializeField] private UnityEvent _positiveZ;
    [SerializeField] private UnityEvent _negativeZ;
    [SerializeField] private bool _canLaunchPlayer;
    public bool Push(Vector3 force)
    {
        if (force.magnitude < _forceThreshhold) return _canLaunchPlayer;
        var direction = force.GetQuantizedDirection();
        if (direction == Vector3.right) { _positiveX.Invoke(); }
        if (direction == Vector3.left) { _negativeX.Invoke(); }
        if (direction == Vector3.up) { _positiveY.Invoke(); }
        if (direction == Vector3.down) { _negativeY.Invoke(); }
        if (direction == Vector3.forward) { _positiveZ.Invoke(); }
        if (direction == Vector3.back) { _negativeZ.Invoke(); }
        return _canLaunchPlayer;
    }

}
