using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameEventRaiser : MonoBehaviour
{
    [Dependency][SerializeField] private GameEvent ge;
    [ContextMenu("Raise()")]
    public void Raise()
    {
        if (ge == null) return;
        ge.Raise();
    }
}
