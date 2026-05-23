using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ArcObject : MonoBehaviour, IBounceable
{
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _duration;
    public float Duration => _duration;
    [SerializeField] private Collider _returnTarget;
    [SerializeField] private SFX _hitSFX;
    [SerializeField] private Transform _destinationIndicator;
    [SerializeField] private UnityEvent _onArcComplete;
    [SerializeField] private UnityEvent _onHitPlayer;
    [SerializeField] private UnityEvent _onHitReturnTarget;
    private void Awake()
    {
        if (_destinationIndicator != null)
            _destinationIndicator.gameObject.SetActive(false);
    }
    private void EnableDestinationIndicator(Vector3 position)
    {
        if (_destinationIndicator != null)
        {
            _destinationIndicator.gameObject.SetActive(true);
            _destinationIndicator.position = position;
        }
    }
    private void DisableDestinationIndicator() { if (_destinationIndicator != null) { _destinationIndicator.gameObject.SetActive(false); } }
    private bool _returning = false;
    public void SetMaxHeight(float height) { _maxHeight = height; }
    public void SetDuration(float duration) { _duration = duration; }
    public void SetTarget(Vector3 targetPosition)
    {
        // arc towards target with maximum height
        // travel along xz plane at constant speed, while y position follows a parabolic arc
        Vector3 startPosition = transform.position;
        StopAllCoroutines();
        StartCoroutine(Co_Arc());
        IEnumerator Co_Arc()
        {
            if (!_returning)
            {
                EnableDestinationIndicator(targetPosition);
            }
            float time = 0;
            while (time < _duration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / _duration);
                // xz position
                Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
                // y position (parabolic arc)
                float height = 4 * _maxHeight * t * (1 - t);
                currentPosition.y += height;
                transform.position = currentPosition;
                yield return null;
            }
            transform.position = targetPosition;
            if (!_returning)
            {
                _onArcComplete.Invoke();
                DisableDestinationIndicator();
            }
            else
            {
                _returning = false;
            }
        }
    }
    public void SetTarget(Transform targetTransform)
    {
        SetTarget(targetTransform.position);
    }
    public bool CanBounce()
    {
        return !_returning;
    }
    public void Bounce()
    {
        if (_returnTarget != null)
        {
            DisableDestinationIndicator();
            _returning = true;
            _duration /= 2f;

            SetTarget(_returnTarget.transform);
        }
    }
    public void TargetPlayer()
    {
        _returning = false;
        SetTarget(PlayerData.Position + Vector3.up);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_returning && other.transform == _returnTarget.transform)
        {

            _hitSFX.PlayAtPoint(transform.position);
            _onHitReturnTarget.Invoke();
        }
        else if (other.CompareTag(PlayerManager.Tag))
        {
            _onHitPlayer.Invoke();
        }
    }
    public void TargetRandomPoint(RandomAreaPointGenerator randomAreaPointGenerator)
    {
        _returning = false;
        SetTarget(randomAreaPointGenerator.GetRandomPoint());
    }
    public void TargetFurthestPointFromPlayer(RandomAreaPointGenerator randomAreaPointGenerator)
    {
        _returning = false;
        Vector3 between = (PlayerData.Position + CompanionData.Position) / 2f;
        SetTarget(randomAreaPointGenerator.GetFurthestPoint(between));
    }
}
