using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(BigTitleAttribute))]
public class BigTitleDrawer : DecoratorDrawer
{
    float _height;
    public override void OnGUI(Rect position)
    {
        BigTitleAttribute b = (BigTitleAttribute)attribute;
        _height = b.Height;
        //position.height = b.Height;
        ExtendedEditor.DrawTitle(position, b.Text, b.FontSize);
        base.OnGUI(position);
    }
    public override float GetHeight()
    {
        float height = _height;

        return base.GetHeight() + height;
    }
}
