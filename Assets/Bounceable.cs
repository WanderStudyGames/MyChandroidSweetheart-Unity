using UnityEngine;
using UnityEngine.Events;

public class Bounceable : MonoBehaviour, IBounceable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private UnityEvent _onBounce;
    [SerializeField] private UnityEvent _onPlayerHit;
    [Tooltip("The impact force required to register a hit on the player.")]
    [SerializeField] private float _playerHitThreshhold = 10f;
    private float _bounceCooldown = 1f;
    private bool _canBounce = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PlayerManager.Tag))
        {
            bool rigidBodyKilling = _rigidbody == null || _rigidbody.velocity.y < 0;
            if (collision.impulse.magnitude > _playerHitThreshhold && rigidBodyKilling)
            {
                _onPlayerHit.Invoke();
            }
        }
    }
    public bool CanBounce()
    {
        if (_rigidbody == null) return true;
        return _rigidbody.velocity.y < 0 && _canBounce;
    }

    public void Bounce()
    {
        if (!_canBounce) return;
        _onBounce?.Invoke();
        if (_rigidbody == null) return;
        _canBounce = false;
        Invoke(nameof(ResetBounce), _bounceCooldown);
        _rigidbody.velocity = new(
            _rigidbody.velocity.x,
            -_rigidbody.velocity.y,
            _rigidbody.velocity.z
            );
    }
    private void ResetBounce()
    {
        _canBounce = true;
    }
}
