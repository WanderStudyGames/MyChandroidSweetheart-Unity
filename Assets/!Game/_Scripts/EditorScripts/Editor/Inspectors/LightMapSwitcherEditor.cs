using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LightMapSwitcher))]
public class LightMapSwitcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button(new GUIContent("Select Lights")))
        {
            var thing = (LightMapSwitcher)target;
            thing.SelectLights();
        }

        if (!Lightmapping.isRunning && GUILayout.Button(new GUIContent("Generate Lighting")))
        {
            var thing = (LightMapSwitcher)target;
            thing.BakeLighting();
        }
        if (Lightmapping.isRunning && GUILayout.Button(new GUIContent("Cancel")))
        {
            Lightmapping.Cancel();
        }
    }
}
