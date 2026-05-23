using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class LocalHandleSceneDrawer
{
    static LocalHandleSceneDrawer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        foreach (var obj in Selection.gameObjects)
        {
            var monoBehaviours = obj.GetComponents<MonoBehaviour>();
            foreach (var mb in monoBehaviours)
            {
                // Only process MonoBehaviours marked with [HasLocalHandles]
                if (mb.GetType().GetCustomAttribute<HasLocalHandlesAttribute>() == null)
                    continue;

                var type = mb.GetType();
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<LocalHandleAttribute>() != null && field.FieldType == typeof(Vector3))
                    {
                        Vector3 localValue = (Vector3)field.GetValue(mb);
                        Vector3 worldValue = mb.transform.TransformPoint(localValue);

                        EditorGUI.BeginChangeCheck();
                        Vector3 newWorldValue = Handles.PositionHandle(worldValue, mb.transform.rotation);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(mb, "Move Local Handle");
                            field.SetValue(mb, mb.transform.InverseTransformPoint(newWorldValue));
                            EditorUtility.SetDirty(mb);

                            // Manually invoke OnValidate if it exists
                            var onValidate = type.GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            if (onValidate != null)
                            {
                                onValidate.Invoke(mb, null);
                            }
                        }
                    }
                }
            }
        }
    }
}