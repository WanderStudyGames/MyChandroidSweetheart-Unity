using UnityEngine;

public class LookAtRandomizer : MonoBehaviour
{
    [SerializeField] private LerpToPlayer _lerpToPlayer;
    [SerializeField] private Animator _animator;
    private IdleTimer _idleTimer;
    void Awake()
    {
        _idleTimer = _animator.GetBehaviour<IdleTimer>();
    }
    void OnEnable()
    {

    }
    void OnDisable()
    {
    }

    private void OnIdle()
    {

    }

}
