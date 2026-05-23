using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootProfile : ComponentProfile
{
    [SerializeField] private GameObject _prefab;
    public GameObject Prefab => _prefab;
}
