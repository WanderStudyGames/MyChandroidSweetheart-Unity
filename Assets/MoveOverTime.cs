using System.Collections;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direction;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Move());
        IEnumerator Co_Move()
        {
            while (true)
            {
                transform.position += _direction.normalized * _speed * Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
