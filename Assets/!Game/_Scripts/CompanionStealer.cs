using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

public class CompanionStealer : MonoBehaviour
{
    [SerializeField] private UnityEvent _executeOnCapture;
    private Companion _companion;
    public static event Action OnCompanionStolen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Companion companion))
        {
            _companion = companion;
            CompanionData.Command(null);
            CompanionManager.DropCarriedItem();
            OnCompanionStolen?.Invoke();
            StartCoroutine(Co_Disable());
            _executeOnCapture.Invoke();
        }
        IEnumerator Co_Disable()
        {
            yield return null;
            companion.AttachTo(gameObject);
            companion.transform.position -= Vector3.one * 100f;
            Physics.SyncTransforms();
            yield return new WaitForFixedUpdate();
            companion.gameObject.SetActive(false);

        }
    }
    public void Release(Transform transformPosition)
    {
        _companion.transform.position = transformPosition.position;
        _companion.gameObject.SetActive(true);
        _companion.Detach();
        _companion = null;
    }
}
