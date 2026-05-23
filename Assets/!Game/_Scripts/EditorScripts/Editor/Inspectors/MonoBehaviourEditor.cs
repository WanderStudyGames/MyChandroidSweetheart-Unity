using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{
    void OnSceneGUI()
    {
        var targetObj = target as MonoBehaviour;
        if (targetObj == null) return;

        var type = targetObj.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<LocalHandleAttribute>() != null && field.FieldType == typeof(Vector3))
            {
                Vector3 localValue = (Vector3)field.GetValue(targetObj);
                Vector3 worldValue = targetObj.transform.TransformPoint(localValue);

                EditorGUI.BeginChangeCheck();
                Vector3 newWorldValue = Handles.PositionHandle(worldValue, targetObj.transform.rotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(targetObj, "Move Local Handle");
                    field.SetValue(targetObj, targetObj.transform.InverseTransformPoint(newWorldValue));
                    EditorUtility.SetDirty(targetObj);
                }
            }
        }
    }
}