using System;
using System.Collections;
using UnityEngine;
//To work on later. Must define start position and end position for each axis.
//Durations currently broken.
public class Lift3D : MonoBehaviour
{
    [Serializable]
    public struct Movement
    {
        public Transform Destination;
        public AnimationCurve Curve;
        public float Duration;
    }
    [SerializeField] private Transform _liftObjectX;
    [SerializeField] private Transform _liftObjectY;
    [SerializeField] private Transform _liftObjectZ;
    [SerializeField] private AttachablePlatform _attachablePlatform;
    [SerializeField] private bool _easeInOut = false;
    private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float _duration = 1f;
    public void SetDuration(float duration) { _duration = duration; }
    private IEnumerator _xCoroutine;
    private IEnumerator _yCoroutine;
    private IEnumerator _zCoroutine;

    private void Awake()
    {
        if (_easeInOut)
        {
            _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }


    private int _dimensionsMoving = 0;

    public void MoveX(Transform destination)
    {
        _xCoroutine = Move(_liftObjectX, destination, _xCoroutine);
    }

    public void MoveY(Transform destination)
    {
        _yCoroutine = Move(_liftObjectY, destination, _yCoroutine);
    }

    public void MoveZ(Transform destination)
    {
        _zCoroutine = Move(_liftObjectZ, destination, _zCoroutine);
    }

    private IEnumerator Move(Transform liftObject, Transform Destination, IEnumerator coroutine)
    {
        //increment only if not already moving in this dimension
        if (coroutine == null)
        {
            _dimensionsMoving++;
            if (_dimensionsMoving == 1)
            {
                _attachablePlatform.AttachNPCS();
                Debug.Log("Attaching NPCs to lift");
            }
        }
        else
        {
            StopCoroutine(coroutine);
        }
        coroutine = Co_Move();
        StartCoroutine(coroutine);
        IEnumerator Co_Move()
        {
            var time = 0f;
            var duration = _duration;
            Vector3 startPosition = liftObject.transform.position;
            Vector3 endPosition = Vector3.zero;

            while (time < duration)
            {
                time += Time.deltaTime;
                Debug.Log($"Curve Evaluate: {_curve.Evaluate(time / duration)} at time {time / duration}");
                liftObject.transform.position = new Vector3(
                    Mathf.Lerp(startPosition.x, endPosition.x, _curve.Evaluate(time / duration)),
                    Mathf.Lerp(startPosition.y, endPosition.y, _curve.Evaluate(time / duration)),
                    Mathf.Lerp(startPosition.z, endPosition.z, _curve.Evaluate(time / duration))
                    );
                yield return null;
            }
            liftObject.transform.position = Vector3.zero;
            End();
        }
        return coroutine;
    }

    private void End()
    {
        Debug.Log($"Dimension movement complete. _dimensionsMoving:{_dimensionsMoving}");
        if (_dimensionsMoving > 0)
        {
            _dimensionsMoving--;
            if (_dimensionsMoving == 0)
            {
                _attachablePlatform.DetachNPCS();
                Debug.Log("Detaching NPCs from lift");
            }
        }
    }


}
