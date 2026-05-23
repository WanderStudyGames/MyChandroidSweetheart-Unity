using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DependencyAttribute : PropertyAttribute
{
    public DependencyAttribute()
    {
    }

}
