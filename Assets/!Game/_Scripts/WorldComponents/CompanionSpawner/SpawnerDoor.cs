using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class SpawnerDoor : MonoBehaviour
{
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float doorOpenSpeed;
    private Vector3 closedRotation;
    private Vector3 destinationRotation;
    private void Start()
    {
        closedRotation = GetComponent<Transform>().localEulerAngles;
        destinationRotation = closedRotation;
    }
    public void Open()
    {
        StopAllCoroutines();
        destinationRotation = openRotation;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(DoorMove());
    }
    public void Close()
    {
        StopAllCoroutines();
        destinationRotation = closedRotation;
        GetComponent<Collider>().enabled = true;
        StartCoroutine(DoorMove());
    }
    private IEnumerator DoorMove()
    {
        while(transform.localEulerAngles != destinationRotation)
        {
            transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, destinationRotation, doorOpenSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
