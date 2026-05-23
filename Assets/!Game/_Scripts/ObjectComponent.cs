using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectComponent : MonoBehaviour, IObjectComponent
{
    public abstract void SetComponentProfile(ComponentProfile profile);
}

public interface IObjectComponent
{
    public abstract void SetComponentProfile(ComponentProfile profile);
}
