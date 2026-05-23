using UnityEngine;
using UnityEngine.Events;

public class MusicNerdBoss : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onTakeDamage;
    [SerializeField] private UnityEvent _on2Health;
    [SerializeField] private UnityEvent _on1Health;
    public void TakeDamage()
    {
        Debug.Log($"Health before damage: {health}");
        health--;
        Debug.Log($"Health after damage: {health}");
        if (health <= 0)
        {
            _onDie.Invoke();
            return;
        }
        _onTakeDamage.Invoke();
        if (health == 2)
        {
            _on2Health.Invoke();
        }
        else if (health == 1)
        {
            _on1Health.Invoke();
        }
    }
}
