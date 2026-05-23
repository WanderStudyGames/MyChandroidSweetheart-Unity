using PathCreation;
using System.Collections;
using UnityEngine;

public class PathSequencer : MonoBehaviour
{
    public void StartPath(PathAnimation pathAnimation)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Path(pathAnimation));
    }
    public void StartPathAtClosestPoint(PathAnimation pathAnimation)
    {
        var startTime = pathAnimation.PathCreator.path.GetClosestTimeOnPath(transform.position);
        StopAllCoroutines();
        StartCoroutine(Co_Path(pathAnimation, startTime));
    }
    private IEnumerator Co_Path(PathAnimation pathAnimation, float startTime = 0)
    {
        var time = startTime;
        while (time < pathAnimation.Duration)
        {
            transform.position = pathAnimation.PathCreator.path.GetPointAtTime(pathAnimation.AnimationCurve.Evaluate(time / pathAnimation.Duration), EndOfPathInstruction.Stop);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = pathAnimation.PathCreator.path.GetPointAtTime(1f, EndOfPathInstruction.Stop);
        pathAnimation.OnFinish?.Invoke();
    }
}
