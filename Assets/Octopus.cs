using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Octopus : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private ArcObject _sprocketable;
    [SerializeField] private ArcObject _urchin;
    [SerializeField] private Transform _arcOrigin;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onTakeDamage;
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            _onDie.Invoke();
            return;
        }
        _onTakeDamage.Invoke();
    }
    public void Throw()
    {

        StopAllCoroutines();
        if (health <= 0) { return; }
        if (health == 3)
        {
            StartCoroutine(Co_ThrowSprocketable());
        }
        if (health == 2)
        {
            StartCoroutine(Co_Throws2());
            IEnumerator Co_Throws2()
            {
                StartCoroutine(Co_ThrowUrchin());
                yield return new WaitForSeconds(_urchin.Duration + 2f);
                StartCoroutine(Co_ThrowUrchin());
                yield return new WaitForSeconds(_urchin.Duration + 2f);
                StartCoroutine(Co_ThrowSprocketable());
            }
        }
        if (health == 1)
        {
            _urchin.SetDuration(1.5f);
            _sprocketable.SetDuration(3f);
            StartCoroutine(Co_ThrowCrazy());
            IEnumerator Co_ThrowCrazy()
            {
                StartCoroutine(Co_ThrowUrchin());
                yield return new WaitForSeconds(_urchin.Duration + 2f);
                StartCoroutine(Co_ThrowUrchin());
                yield return new WaitForSeconds(0.7f);
                StartCoroutine(Co_ThrowSprocketable());
            }

        }
    }
    private IEnumerator Co_ThrowSprocketable()
    {
        yield return new WaitForSeconds(2f);
        _sprocketable.transform.position = _arcOrigin.position;
        _sprocketable.TargetPlayer();
    }
    private IEnumerator Co_ThrowUrchin()
    {
        yield return new WaitForSeconds(2f);
        _urchin.transform.position = _arcOrigin.position;
        _urchin.TargetPlayer();
    }
    private void Start()
    {
        Throw();
    }
}
