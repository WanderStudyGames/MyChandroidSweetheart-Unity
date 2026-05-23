using UnityEngine;
using UnityEngine.Events;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceThreshold = 5f;
    [SerializeField] private UnityEvent _onSuccess;
    public void Check()
    {
        if (Vector3.Distance(origin.position, target.position) <= distanceThreshold)
        {
            _onSuccess.Invoke();
        }
    }
}
