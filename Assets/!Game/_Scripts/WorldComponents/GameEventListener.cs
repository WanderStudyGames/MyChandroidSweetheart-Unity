using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class GameEventResponse
{
    [SerializeField] private GameEvent gameEvent;
    public GameEvent GameEvent => gameEvent;
    [SerializeField] private UnityEvent response;
    public void Subscribe() { gameEvent.AddListener(this); }
    public void Unsubscribe() { gameEvent.RemoveListener(this); }
    public void OnEventRaised() { response.Invoke(); }
}
public class GameEventListener : MonoBehaviour
{
    [SerializeField] private List<GameEventResponse> gEResponses;

    public void OnEnable()
    {
        for (int i = gEResponses.Count - 1; i >= 0; i--)
        {
            if (gEResponses[i].GameEvent == null) { Debug.LogError($"{gameObject.name}: {this.GetType().Name} null reference exception"); }
            gEResponses[i].Subscribe();

        }
    }
    public void OnDisable()
    {
        for (int i = gEResponses.Count - 1; i >= 0; i--)
        {
            gEResponses[i].Unsubscribe();

        }
    }
}
