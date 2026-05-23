using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 offset = Vector3.zero;
    
    public void Instantiate()
    {
        GameObject.Instantiate(prefab, transform.position + offset, Quaternion.identity);
    }
}
