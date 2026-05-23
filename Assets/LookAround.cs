using System.Collections;

using UnityEngine;

using VInspector;

public class LookAround : MonoBehaviour
{
    [SerializeField] private StateGroupManager _stateGroupManager;
    [SerializeField] private LerpToPlayer _lerpToPlayer;
    [SerializeField] private Transform _root;
    [SerializeField] private float _radius = 25f;
    [SerializeField, MinMaxSlider(-90f, 90f)] private Vector2 _range;
    [SerializeField] private float _maxHeight = 5f;

    void OnEnable()
    {
        _stateGroupManager.OnGroupEnter += OnGroupEnter;
        _stateGroupManager.OnGroupExit += OnGroupExit;
        CompanionOverrideController.OnReactStart += OnReactStart;
        CompanionOverrideController.OnReactEnd += OnReactEnd;
    }
    void OnDisable()
    {
        _stateGroupManager.OnGroupEnter -= OnGroupEnter;
        _stateGroupManager.OnGroupExit -= OnGroupExit;
        CompanionOverrideController.OnReactStart -= OnReactStart;
        CompanionOverrideController.OnReactEnd -= OnReactEnd;
    }
    private void OnReactStart()
    {
        StopAllCoroutines();
    }
    private void OnReactEnd()
    {
        Look();
    }
    private void OnGroupEnter(int ID)
    {
        if (ID == 0 && !CompanionOverrideController.IsReacting)
            Look();
    }
    public void OnInteract()
    {
        _lerpToPlayer.TargetPlayer(true);
        Look();
    }
    private void Look()
    {
        StopAllCoroutines();
        StartCoroutine(Co_LookAround());
        StartCoroutine(Co_Sit());
        IEnumerator Co_LookAround()
        {
            _lerpToPlayer.TargetPlayer();
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            while (true)
            {
                if (Random.Range(0, 3) == 0)
                {
                    _lerpToPlayer.TargetPlayer();
                    yield return new WaitForSeconds(Random.Range(2f, 4f));
                }
                else
                {
                    var yaw = Random.Range(_range.x, _range.y);
                    var forward = Vector3.ProjectOnPlane(_root.forward, _root.up).normalized;
                    var dir = Quaternion.AngleAxis(yaw, _root.up) * forward;
                    var pos = _root.position + dir * _radius;
                    pos.y += Random.Range(0, _maxHeight);
                    _lerpToPlayer.SetTarget(pos, Random.Range(10f, 14f));
                    yield return new WaitForSeconds(Random.Range(3f, 8f));
                }
            }
        }
        IEnumerator Co_Sit()
        {
            yield return new WaitForSeconds(60);
            CompanionManager.Sit();
        }
    }
    private void OnGroupExit(int ID)
    {
        if (ID != 0) return;
        StopAllCoroutines();
        if (!CompanionOverrideController.IsReacting)
            _lerpToPlayer.TargetPlayer();
    }
    void OnDrawGizmosSelected()
    {
        var yaw = (_range.x + _range.y) / 2;
        var forward = Vector3.ProjectOnPlane(_root.forward, _root.up).normalized;
        var dir = Quaternion.AngleAxis(yaw, _root.up) * forward;
        var pos = _root.position + dir * _radius;
        pos.y += _maxHeight;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_root.position, pos);
        Gizmos.DrawWireSphere(pos, 1f);
    }
}
