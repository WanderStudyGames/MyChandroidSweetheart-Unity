using PathCreation;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PathTraveler : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _travelTime;
    [SerializeField] private UnityEvent _onFinishTravelForward;
    [SerializeField] private UnityEvent _onFinishTravelBackward;

    public void TravelForward()
    {
        Travel(_travelTime, new(_pathCreator.path.GetClosestTimeOnPath(transform.position), 1), () => { Debug.Log("FORWARD DONE"); _onFinishTravelForward.Invoke(); });
    }
    public void Travel(float travelTime, Vector2 between, Action onFinish = null, bool faceBackward = false)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Travel());
        IEnumerator Co_Travel()
        {

            yield return ExtensionMethods.Co_FadeFloat(travelTime, between, fl =>
            {
                transform.position = _pathCreator.path.GetPointAtTime(fl, EndOfPathInstruction.Stop);
                transform.rotation = _pathCreator.path.GetRotation(fl, EndOfPathInstruction.Stop);
                if (faceBackward) transform.Rotate(Vector3.up, 180f, Space.Self);
            });
            Debug.Log("FINISH");
            onFinish?.Invoke();
        }
    }
    public void TravelBackward()
    {
        Travel(_travelTime, new(_pathCreator.path.GetClosestTimeOnPath(transform.position), 0), () => { _onFinishTravelBackward.Invoke(); }, true);
    }
}
