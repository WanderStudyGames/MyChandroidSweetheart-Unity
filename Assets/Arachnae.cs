using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Arachnae : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private LerpOnCommand _ceilingNodes;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _onThrowProjectile;
    [SerializeField] private UnityEvent _onCeiling;
    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            _onDie.Invoke();
            return;
        }
        _onTakeDamage.Invoke();
        StartCoroutine(Co_Sequence());
        IEnumerator Co_Sequence()
        {
            yield return new WaitForSeconds(2f);
            var count = 0;
            var iterations = Random.Range(5, 10);
            while (count < iterations)
            {
                count++;
                yield return new WaitForSeconds(1f);
                _onThrowProjectile.Invoke();
            }
            MoveToCeilingNode();
            yield return new WaitForSeconds(2f);
            _onCeiling.Invoke();
            while (true)
            {
                yield return new WaitForSeconds(2f);
                MoveToCeilingNode();
            }


        }

    }
    private void MoveToCeilingNode()
    {
        var choice = 0;
        do
        {
            choice = Random.Range(0, _ceilingNodes.TargetCount - 1);

        } while (choice == _ceilingNodes.CurrentIndex);
        _ceilingNodes.SetDestination(choice);
    }
    public void OnGrabbed()
    {
        StopAllCoroutines();
    }
}
