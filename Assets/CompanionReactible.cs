using System;

using UnityEngine;

public class CompanionReactible : MonoBehaviour
{
    [SerializeField] private CompanionReaction _reaction;
    public CompanionReaction Reaction => _reaction;
    [SerializeField] private bool _lookAt = true;
    public bool LookAt => _lookAt;
    public event Action<CompanionReactible> BeforeDisable;
    private void OnDisable()
    {
        BeforeDisable?.Invoke(this);
    }

}