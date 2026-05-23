using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DependencyFoldoutAttribute : PropertyAttribute
{
    public DependencyFoldoutAttribute()
    {

    }
}
