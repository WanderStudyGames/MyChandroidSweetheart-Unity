using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class TextDataAttribute : PropertyAttribute
{
    public string PathRoot { get; private set; }
    public string NameStarter { get; private set; }
    public TextDataAttribute(string pathRoot, string nameStarter)
    {
        PathRoot = pathRoot;
        NameStarter = nameStarter;
    }
}