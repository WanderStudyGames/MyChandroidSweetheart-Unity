using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoOpenDoor : MonoBehaviour
{
    [SerializeField] private LayerMask _autoOpenLayers;
    [SerializeField] private LayerMask _autoCloseLayers;
    [SerializeField] private LerpOnCommand _lerpOnCommand;
    [SerializeField] private UnityEvent _openDoorEvent;
    [SerializeField] private UnityEvent _closeDoorEvent;
    private List<string> colliders = new();
    private bool doorOpen;
    public void OpenDoor()
    {
        doorOpen = true;
        _lerpOnCommand?.SetDestination(1);
        _openDoorEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_autoCloseLayers.Contains(other.gameObject.layer))
        {
            StopAllCoroutines();
            colliders.AddUnique(other.gameObject.name);
        }
        if (_autoOpenLayers.Contains(other.gameObject.layer) && !doorOpen) { OpenDoor(); }

    }
    public void CloseDoor()
    {
        _closeDoorEvent.Invoke();
        doorOpen = false;
        _lerpOnCommand?.SetDestination(0);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_autoCloseLayers.Contains(other.gameObject.layer))
            colliders.RemoveAll(other.gameObject.name);
        if (colliders.Count == 0 && doorOpen)
        {
            StopAllCoroutines();
            StartCoroutine(Co_DoorClose());
            IEnumerator Co_DoorClose()
            {
                yield return new WaitForSeconds(1f);
                CloseDoor();
            }
        }

    }
}
