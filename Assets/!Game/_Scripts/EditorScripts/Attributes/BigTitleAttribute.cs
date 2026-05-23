using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class BigTitleAttribute : PropertyAttribute
{
    public string Text;
    public float Height;
    public int FontSize;
    public BigTitleAttribute(string title, float height = 20f, int fontSize = 20)
    {
        Text = title;
        Height = height;
        FontSize = fontSize;
    }
}
