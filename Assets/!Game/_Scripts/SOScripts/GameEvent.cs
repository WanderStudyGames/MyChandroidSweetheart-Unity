using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventResponse> listeners = new();
    public Action Action;

    public void AddListener(GameEventResponse l) { listeners.Add(l); }
    public void RemoveListener(GameEventResponse l) { listeners.Remove(l); }

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
        Action?.Invoke();
    }

}