using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTargetedProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private UnityEvent _onLaunch;
    public void Launch()
    {
        Vector3 direction = Vector3.Normalize(PlayerData.Position - transform.position);
        StopAllCoroutines();
        StartCoroutine(Co_Move());
        _onLaunch.Invoke();
        IEnumerator Co_Move()
        {
            while (true)
            {
                transform.position += (direction * _speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
