using System.Collections;
using UnityEngine;

public class ShadowDrop : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Co_Enable());
        IEnumerator Co_Enable()
        {
            yield return null;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                transform.position = hit.point + (Vector3.Normalize(hit.normal) * 0.01f);
                transform.LookAt(hit.point - hit.normal);
            }
        }
    }
}
