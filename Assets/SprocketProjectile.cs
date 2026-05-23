using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SprocketProjectile : MonoBehaviour, ISprocketPushable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private UnityEvent _onPush;
    [SerializeField] private UnityEvent _onDespawn;
    public bool Push(Vector3 force)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        _onPush.Invoke();
        StopAllCoroutines();
        StartCoroutine(Co_Despawn());
        return false;
        IEnumerator Co_Despawn()
        {
            yield return new WaitForSeconds(2f);
            rb.isKinematic = true;
            rb.useGravity = false;
            _onDespawn.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
