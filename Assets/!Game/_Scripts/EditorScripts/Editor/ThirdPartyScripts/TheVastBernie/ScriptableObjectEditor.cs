using UnityEditor;
using UnityEngine;

/// https://forum.unity.com/threads/better-scriptableobjects-inspector-editing-editor-tool.484392/
///see ScriptableObjectDrawer
[CanEditMultipleObjects]
[CustomEditor(typeof(ScriptableObject), true)]
public class ScriptableObjectEditor : Editor
{
}