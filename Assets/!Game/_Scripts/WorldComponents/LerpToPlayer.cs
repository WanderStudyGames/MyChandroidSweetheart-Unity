using System.Collections;

using UnityEngine;

public class LerpToPlayer : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private bool TargetCompanion;
    public void SetTargetCompanion(bool b)
    {
        TargetCompanion = b;
        StartLerp();
    }
    private void OnEnable()
    {
        StartLerp();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void StartLerp()
    {
        StopAllCoroutines();
        if (TargetCompanion && CompanionManager.CompanionWithPlayer)
            StartCoroutine(Co_TargetCompanion());
        else
            StartCoroutine(Co_TargetPlayer());

        IEnumerator Co_TargetPlayer()
        {
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, PlayerData.Position + Offset, lerpSpeed * Time.deltaTime);
                yield return null;
            }

        }
        IEnumerator Co_TargetCompanion()
        {
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, CompanionData.Position + Offset, lerpSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
    public void TargetPlayer(bool instantTransition = false)
    {
        TargetCompanion = false;
        StopAllCoroutines();
        if (instantTransition) transform.position = PlayerData.Position + Offset;
        StartLerp();
    }
    public void SetTarget(Transform target, float speed = 5f, Vector3 offset = default)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Target(target));
        IEnumerator Co_Target(Transform target)
        {
            while (target != null)
            {
                transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
    private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public void SetTarget(Vector3 location, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Target(location));
        IEnumerator Co_Target(Vector3 location)
        {
            var t = 0f;
            var start = transform.position;
            var time = Vector3.Distance(start, location) / speed;
            while (t < time)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(start, location, _curve.Evaluate(t / time));
                yield return null;
            }
        }
    }
}