using System.Collections;
using UnityEngine;

public class PlayerHandTransformProvider : MonoBehaviour
{
    [SerializeField] private Transform _handUp;
    [SerializeField] private Transform _handDown;

    private IEnumerator Start()
    {
        yield return null;
        PlayerData.UIHandUp = _handUp;
        PlayerData.UIHandDown = _handDown;
    }
}
