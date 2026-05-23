using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MovingBlock : MonoBehaviour, ISprocketPushable
{
    [SerializeField] private float _moveSpeed = 1000f;
    [SerializeField][Range(0, 1)] private float _forceThreshhold = 1f;
    [SerializeField] private SFX _endSFX;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Stop()
    {
        _rigidbody.velocity = Vector3.down * 100f;
        _rigidbody.solverIterations = 10;

        NavMeshRebake.Rebake();
    }

    public bool Push(Vector3 force)
    {
        if (force.magnitude < _forceThreshhold) return true;

        StopAllCoroutines();
        StartCoroutine(Co_Move());
        IEnumerator Co_Move()
        {
            _rigidbody.WakeUp();
            var quantized = force.GetQuantizedDirection();
            if (quantized.normalized != Vector3.up)
            {
                _rigidbody.AddForce(quantized * _moveSpeed);
            }
            else yield break;

            yield return new WaitUntil(() => _rigidbody.velocity != Vector3.zero);
            yield return new WaitUntil(() => _rigidbody.velocity == Vector3.zero);

            NavMeshRebake.Rebake();

            _endSFX?.PlayAtPoint(transform.position);
        }
        return false;
    }
}
