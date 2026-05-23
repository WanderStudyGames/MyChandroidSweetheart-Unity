using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    public static Color Color = new(0f, 0f, 0f, 0.2f);
    public override void OnGUI(Rect position)
    {
        SeparatorAttribute sa = (SeparatorAttribute)attribute;

        Rect r = new(position.xMin, position.yMin + sa.Spacing, position.width, sa.Height);

        //EditorGUI.DrawRect(r, Color);
        DrawSeparator(position, sa.Height, sa.Spacing);
    }

    public override float GetHeight()
    {
        SeparatorAttribute sa = (SeparatorAttribute)attribute;
        return (sa.Spacing * 2) + sa.Height;
    }
    public static void DrawSeparator(Rect position, float height, float spacing, float offset = 0)
    {
        //Rect position = ExtendedEditor.GetRect();
        Rect r = new(position.xMin, position.yMin + spacing + offset, position.width, height);
        EditorGUI.DrawRect(r, Color);
        //EditorGUILayout.Space((spacing * 2) + height);
    }
}
