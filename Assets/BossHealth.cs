using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private SFX _hitSFX;
    public int Health => _health;

    // let boss decide if it will take damage or return the volley

    public void TakeDamage()
    {
        _health--;
        _hitSFX.PlayAtPoint(transform.position);
    }


}
