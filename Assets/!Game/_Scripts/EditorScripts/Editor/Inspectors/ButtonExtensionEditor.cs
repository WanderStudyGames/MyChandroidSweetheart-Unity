using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonExtension))]

public class ButtonExtensionEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        ExtendedEditor.DrawScript(serializedObject);
        base.OnInspectorGUI();
        serializedObject.Update();

        ExtendedEditor.DrawProperty("_onPointerEnter", serializedObject);
        ExtendedEditor.DrawProperty("_onPointerExit", serializedObject);
        ExtendedEditor.DrawProperty("_onSelect", serializedObject);
        ExtendedEditor.DrawProperty("_onDeselect", serializedObject);

        serializedObject.ApplyModifiedProperties();
    }
}