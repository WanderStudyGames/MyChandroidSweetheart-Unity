using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using Unity.VisualScripting;

public class InstantColliders : EditorWindow
{
    static bool enabled = false;
    [MenuItem("Instant Colliders/Toggle")]
    public static void Toggle()
    {
        enabled = !enabled;
        if (enabled) SceneView.duringSceneGui += OnSceneGUI;
        else SceneView.duringSceneGui -= OnSceneGUI;
        Debug.Log($"{nameof(InstantColliders)} {enabled}.");

    }
    private static void Enable()
    {
        if (!enabled)
        {
            enabled = true;
            SceneView.duringSceneGui += OnSceneGUI;
        }
        Debug.Log($"{nameof(InstantColliders)} {enabled}.");
    }
    //private void OnSelectionChange()
    //{
    //    Enable();
    //}
    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();

        ExtendedEditor.FS();
        ExtendedEditor.Hor(() =>
        {
            if (GUILayout.Button("Box"))
            {
                if (Selection.activeObject != null && Selection.activeObject as GameObject != null)
                {
                    Undo.RecordObjects(Selection.objects, nameof(InstantColliders));
                    foreach (GameObject go in Selection.objects)
                    {
                        if (go.GetComponent<Collider>() == null)
                        {
                            go.AddComponent<BoxCollider>();
                            EditorUtility.SetDirty(go);
                            EditorSceneManager.MarkSceneDirty(go.GameObject().scene);
                        }
                    }


                }

            }
            if (GUILayout.Button("Mesh"))
            {
                if (Selection.activeObject != null && Selection.activeObject as GameObject != null)
                {
                    Undo.RecordObjects(Selection.objects, nameof(InstantColliders));
                    foreach (GameObject go in Selection.objects)
                    {
                        if (go.GetComponent<Collider>() == null)
                        {
                            go.AddComponent<MeshCollider>();
                            EditorUtility.SetDirty(go);
                            EditorSceneManager.MarkSceneDirty(go.GameObject().scene);
                        }
                    }
                }
            }
            ExtendedEditor.FS();
        });



        Handles.EndGUI();
    }
}