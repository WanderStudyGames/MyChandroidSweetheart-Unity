using System.Collections;
using UnityEngine;

public class LerpToRectTransform : MonoBehaviour
{
    [SerializeField] private RectTransform _targetTransform;
    [SerializeField] private float _speed;
    private Vector3 _startingPosition;
    private IEnumerator Co_Align()
    {
        while (transform.position != _targetTransform.position)
        {
            transform.position = Vector2.Lerp(transform.position, _targetTransform.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
    private void Awake()
    {
        _startingPosition = transform.position;
    }
    private void OnEnable()
    {
        StartCoroutine(Co_Align());
    }
    private void OnDisable()
    {
        transform.position = _startingPosition;
    }
}
