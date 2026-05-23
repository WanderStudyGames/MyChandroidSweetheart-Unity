using UnityEngine;
[RequireComponent(typeof(Animator))]
public class RandomizeAnimFloat : MonoBehaviour
{
    [SerializeField] private string parameterName;

    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _animator.SetFloat(parameterName, Random.Range(0.0f, 1.0f));
    }
}
