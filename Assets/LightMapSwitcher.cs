using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightMapSwitcher : MonoBehaviour
{
    [System.Serializable]
    private class LightMapImageList
    {
        public Texture2D[] light;
        public Texture2D[] dir;
        public LightmapData[] data;
    }
    [SerializeField] private LightMapImageList _lightmapNormal;
    [SerializeField] private LightMapImageList _lightmapDark;
    private bool _lightsOn = true;

    public void Switch(bool b)
    {
        if (_lightsOn == b) return;
        _lightsOn = b;
        List<LightmapData> lightmaps = new();
        var lightmap = b ? _lightmapNormal : _lightmapDark;
        for (int j = 0; j < lightmap.light.Length; j++)
        {
            var data = new LightmapData();
            data.lightmapColor = lightmap.light[j];
            if (j < lightmap.dir.Length)
                data.lightmapDir = lightmap.dir[j];
            lightmaps.Add(data);
        }
        LightmapSettings.lightmaps = lightmaps.ToArray();
    }
#if UNITY_EDITOR
    [SerializeField] private Light[] _variableLights;
    [ContextMenu("SelectLights()")]
    public void SelectLights()
    {
        List<GameObject> list = new();
        foreach (var light in _variableLights)
        {
            if (light != null)
                list.Add(light.gameObject);
        }
        Selection.objects = list.ToArray();
    }
    [ContextMenu("BakeLighting()")]
    public void BakeLighting()
    {
        foreach (var light in _variableLights)
        {
            if (light != null)
                light.enabled = false;
        }
        var path = SceneManager.GetActiveScene().path.Replace(".unity", "/dark");
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateFolder(path.AscendDir(), "dark");
        Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
        Lightmapping.bakeCompleted += Step2;
        Lightmapping.BakeAsync();
        void Step2()
        {
            Lightmapping.bakeCompleted -= Step2;
            var darkDir = new List<Texture2D>();
            var darkLight = new List<Texture2D>();
            var normalDir = new List<Texture2D>();
            var normalLight = new List<Texture2D>();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            var strs = AssetDatabase.FindAssets("comp_dir", new[] { path.AscendDir() });
            if (strs.Length == 0)
            {
                Debug.LogError("OOOPSSSS");
            }
            foreach (var str in strs)
            {
                var itemPath = AssetDatabase.GUIDToAssetPath(str);
                AssetDatabase.CopyAsset(itemPath, path + "/" + itemPath.DirItem());
                darkDir.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/" + itemPath.DirItem()));
            }
            var strs2 = AssetDatabase.FindAssets("comp_light", new[] { path.AscendDir() });
            foreach (var str in strs2)
            {
                var itemPath = AssetDatabase.GUIDToAssetPath(str);
                AssetDatabase.CopyAsset(itemPath, path + "/" + itemPath.DirItem());
                darkLight.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/" + itemPath.DirItem()));
            }

            _lightmapDark.dir = darkDir.ToArray();
            _lightmapDark.light = darkLight.ToArray();

            foreach (var light in _variableLights)
            {
                if (light != null)
                    light.enabled = true;
            }
            Lightmapping.bakeCompleted += Step3;
            Lightmapping.BakeAsync();
            void Step3()
            {
                Lightmapping.bakeCompleted -= Step3;
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                foreach (var str in strs)
                {
                    var itemPath = AssetDatabase.GUIDToAssetPath(str);
                    normalDir.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path.AscendDir() + "/" + itemPath.DirItem()));
                }
                foreach (var str in strs2)
                {
                    var itemPath = AssetDatabase.GUIDToAssetPath(str);
                    normalLight.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(path.AscendDir() + "/" + itemPath.DirItem()));
                }
                _lightmapNormal.dir = normalDir.ToArray();
                _lightmapNormal.light = normalLight.ToArray();
                EditorUtility.SetDirty(this);
            }
        }

    }
#endif
}
