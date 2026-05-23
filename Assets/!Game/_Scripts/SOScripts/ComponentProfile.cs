using System;
using UnityEngine;

public class ComponentProfile : ScriptableObject
{
    public Type GetComponentType() => ProfileFor.GetComponentTypeFromProfile(this);
}