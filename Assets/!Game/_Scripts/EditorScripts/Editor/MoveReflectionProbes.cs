using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class MoveReflectionProbes
{
    [MenuItem("Cubemaps/Bake")]
    private static void TryAgain()
    {
        var path = SceneManager.GetActiveScene().path.AscendDir() + "/Probes/";
        var sceneName = SceneManager.GetActiveScene().name;
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
        int i = 0;
        RenderSettings.reflectionBounces = Mathf.Max(2, RenderSettings.reflectionBounces);
        foreach (ReflectionProbe rp in GameObject.FindObjectsOfType<ReflectionProbe>())
        {
            if (rp.mode != UnityEngine.Rendering.ReflectionProbeMode.Custom) continue;
            rp.renderDynamicObjects = true;
            Lightmapping.BakeReflectionProbe(rp, path + sceneName + i + ".png");
            EditorUtility.SetDirty(rp);
            PrefabUtility.RecordPrefabInstancePropertyModifications(rp);
            i++;
        }
    }
}
