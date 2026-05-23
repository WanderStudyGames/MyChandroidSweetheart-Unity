using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private bool _lockXZAxis = true;
    private void Update()
    {
        Quaternion target;
        target = Quaternion.LookRotation(PlayerData.Position - transform.position, Vector3.up);
        if (_lockXZAxis)
            target = Quaternion.Euler(0, target.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, _speed * Time.deltaTime);
    }
}
