using UnityEngine;

public class SetBustPhysics : MonoBehaviour
{
    [SerializeField] private SoxAtkJiggleBone _jiggleBone;
    [SerializeField] private SoxAtkJiggleBone _buttJiggleBone;
    [SerializeField] private CompanionData _companionData;
    private void Awake()
    {
        Set();
    }
    private void OnEnable()
    {
        CompanionData.OnBustPhysicsChanged += Set;
    }
    private void OnDisable()
    {
        CompanionData.OnBustPhysicsChanged -= Set;
    }
    private void Set()
    {
        _jiggleBone.enabled = _companionData.BustPhysics;
        _buttJiggleBone.enabled = _companionData.BustPhysics;
    }
}
