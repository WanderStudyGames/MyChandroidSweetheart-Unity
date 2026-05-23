using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScienceNerd : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private GameObject _drones1;
    [SerializeField] private GameObject _drones2;
    [SerializeField] private GameObject _drones3;
    [SerializeField] private UnityEvent _onLaser;

    private void Awake()
    {
        Setup();
    }
    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            _onDie.Invoke();
        }
        Setup();
    }
    private void Setup()
    {
        _drones1.SetActive(false);
        _drones2.SetActive(false);
        _drones3.SetActive(false);
        switch (_health)
        {
            case 3:
                StartCoroutine(Co_Phase1());
                break;
            case 2:
                StartCoroutine(Co_Phase2());
                break;
            case 1:
                StartCoroutine(Co_Phase3());
                break;
            default:
                break;
        }
        IEnumerator Co_Phase1()
        {
            yield return new WaitForSeconds(2f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _drones1.SetActive(true);
        }
        IEnumerator Co_Phase2()
        {
            yield return new WaitForSeconds(2f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _drones2.SetActive(true);
        }
        IEnumerator Co_Phase3()
        {
            yield return new WaitForSeconds(2f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _onLaser.Invoke();
            yield return new WaitForSeconds(5f);
            _drones3.SetActive(true);
        }
    }
}
