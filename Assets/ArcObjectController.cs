using UnityEngine;
using UnityEngine.Events;

public class ArcObjectController : MonoBehaviour
{
    [SerializeField] private int _health = 6;
    [SerializeField] private SFX _hitSFX;
    [SerializeField] private ArcObject _arcObject;
    [SerializeField] private Transform _arcOrigin;
    [SerializeField] private float _maxHeight1 = 4f;
    [SerializeField] private float _maxHeight2 = 8f;
    [SerializeField] private float _duration1 = 2f;
    [SerializeField] private float _duration2 = 1.4f;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onDeath;
    private void Awake()
    {
        Invoke(nameof(TargetPlayer), 3f);
    }
    private void Setup()
    {
        if (_health <= 2)
        {
            _arcObject.SetMaxHeight(_maxHeight2);
            _arcObject.SetDuration(_duration2);
        }
        else
        {
            _arcObject.SetMaxHeight(_maxHeight1);
            _arcObject.SetDuration(_duration1);
        }

        _arcObject.transform.position = _arcOrigin.position;
    }
    public void TargetPlayer()
    {
        Setup();
        _arcObject.TargetPlayer();
    }
    public void TargetRandom(RandomAreaPointGenerator randomAreaPointGenerator)
    {
        Setup();
        _arcObject.TargetFurthestPointFromPlayer(randomAreaPointGenerator);
    }

    public void TakeDamage()
    {
        _health--;
        if (_health == 2 || _health == 4)
        {
            TargetPlayer();
            return;
        }
        else if (_health <= 0)
        {
            _arcObject.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _onDeath.Invoke();
            return;
        }
        _hitSFX.PlayAtPoint(transform.position);
        _onTakeDamage.Invoke();
    }

    //if health is specifically 2 or 4, make boss return the object immediately upon getting hit, no waiting
}
