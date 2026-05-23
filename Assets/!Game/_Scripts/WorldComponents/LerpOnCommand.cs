using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class LerpOnCommand : MonoBehaviour
{
    [SerializeField] private float durationInSeconds = 1;
    [Tooltip("If true, duration is treated as speed (units per second) instead of time.")]
    [SerializeField] private bool _useSpeed = false;
    [SerializeField] private AnimationCurve lerpCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AudioMixerUpdateMode _timeScale = AudioMixerUpdateMode.Normal;
    [SerializeField] private Transform[] targetTransforms;
    public int TargetCount => targetTransforms.Length;
    [SerializeField] private bool lerpPosition = true;
    [SerializeField] private bool lerpRotation = true;
    [SerializeField] private bool lerpScale = true;
    [SerializeField] UnityEvent executeOnFinish;
    private Transform destinationTransform;
    private int index = 0;
    public int CurrentIndex => index;
    public void SetDestination(int i)
    {
        index = i;
        if (i > targetTransforms.Length - 1 || i < 0) return;
        destinationTransform = targetTransforms[i];
        StopAllCoroutines();
        StartCoroutine(Co_MoveTimed(durationInSeconds));
    }
    public void MoveNext()
    {
        index++;
        if (index > targetTransforms.Length - 1) index = 0;
        SetDestination(index);
    }
    IEnumerator Co_MoveTimed(float duration)
    {
        var time = 0f;
        if (_useSpeed)
        {
            //calculate duration based on distance and speed
            float distance = Vector3.Distance(transform.position, destinationTransform.position);
            duration = distance / duration; //convert speed to time
        }
        var pos = transform.position;
        var rot = transform.rotation;
        var scale = transform.localScale;
        while (time <= duration && duration > 0)
        {
            if (lerpPosition) transform.position = Vector3.Lerp(pos, destinationTransform.position, lerpCurve.Evaluate(time / duration));
            if (lerpRotation) transform.rotation = Quaternion.Lerp(rot, destinationTransform.rotation, lerpCurve.Evaluate(time / duration));
            if (lerpScale) transform.localScale = Vector3.Lerp(scale, destinationTransform.localScale, lerpCurve.Evaluate(time / duration));
            time += (_timeScale == AudioMixerUpdateMode.Normal) ? Time.deltaTime : Time.unscaledDeltaTime;
            yield return null;
        }
        if (lerpPosition) transform.position = Vector3.Lerp(pos, destinationTransform.position, 1);
        if (lerpRotation) transform.rotation = Quaternion.Lerp(rot, destinationTransform.rotation, 1);
        if (lerpScale) transform.localScale = Vector3.Lerp(scale, destinationTransform.localScale, 1);

        executeOnFinish.Invoke();
    }
#if UNITY_EDITOR
    [ContextMenu("Move to 1")]
    private void MoveToOne()
    {
        SetDestination(1);
    }
#endif
    public void TeleportTo(int i)
    {
        index = i;
        if (i > targetTransforms.Length - 1 || i < 0) return;
        destinationTransform = targetTransforms[i];
        transform.SetPositionAndRotation(destinationTransform.position, destinationTransform.rotation);
        transform.localScale = destinationTransform.localScale;
        StopAllCoroutines();
    }
}
