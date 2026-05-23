using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class CompanionBehaviorResponse
{
    [SerializeField] private CompanionBehavior _companionBehavior;
    public CompanionBehavior CompanionBehavior => _companionBehavior;
    [SerializeField] private UnityEvent response;
    public void Subscribe() { CompanionBehaviorManager.OnBehaviorChanged += OnStateChanged; }
    public void Unsubscribe() { CompanionBehaviorManager.OnBehaviorChanged -= OnStateChanged; }
    private void OnStateChanged(CompanionBehavior state) { if (state == _companionBehavior) response.Invoke(); }
}
public class CompanionBehaviorListener : MonoBehaviour
{
    [SerializeField] private List<CompanionBehaviorResponse> _behaviorChangeResponses;
    public void OnEnable()
    {
        for (int i = _behaviorChangeResponses.Count - 1; i >= 0; i--)
        {
            if (_behaviorChangeResponses[i].CompanionBehavior == null) { Debug.LogError($"{gameObject.name}: {this.GetType().Name} null reference exception"); }
            _behaviorChangeResponses[i].Subscribe();

        }
    }
    public void OnDisable()
    {
        for (int i = _behaviorChangeResponses.Count - 1; i >= 0; i--)
        {
            _behaviorChangeResponses[i].Unsubscribe();
        }
    }
}
