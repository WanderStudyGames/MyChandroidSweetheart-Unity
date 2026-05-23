using UnityEngine;

public class ChainOnEnable : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private void OnEnable()
    {
        _target.SetActive(true);
    }
    private void OnDisable()
    {
        _target.SetActive(false);
    }
}
