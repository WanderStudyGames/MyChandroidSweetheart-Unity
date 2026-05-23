using UnityEngine;
using UnityEngine.Events;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private BouncySkirtProfile bouncySkirtProfile;
    [SerializeField] private UnityEvent _onBounce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerManager.Tag)
        {

            if (!other.TryGetComponent(out CharacterControllerMove playerMove)) return;

            Vector3 velocity = playerMove.Velocity;
            if (velocity.y > bouncySkirtProfile.VelocityThreshold) return;
            other.GetComponent<CharacterControllerMove>().SetVelocity(new(velocity.x, -velocity.y, velocity.z));
            _onBounce.Invoke();

        }
    }
}
