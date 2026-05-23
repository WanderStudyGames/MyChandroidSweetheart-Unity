using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class TextureFromCubeMapPopup : EditorWindow
{
    public static Texture2D Open(Cubemap cubemap)
    {
        TextureFromCubeMapPopup modal = CreateInstance<TextureFromCubeMapPopup>();
        modal.Setup(cubemap);
        modal.ShowPopup();
        return modal._texture;
    }
    public void Setup(Cubemap cubemap)
    {
        _cubemap = cubemap;
        _cubemapPath = AssetDatabase.GetAssetPath(cubemap);

        //get cubemap importer and adjust settings to allow texture copy
        _importer = AssetImporter.GetAtPath(_cubemapPath) as TextureImporter;
        SetImporterSettings(_importer, _cubemapPath, true, 2048);
    }
    private Texture2D _texture;
    private Cubemap _cubemap;
    private string _cubemapPath;
    private TextureImporter _importer;
    private Vector2 _scrollPos;
    private void OnGUI()
    {

        _scrollPos = GUILayout.BeginScrollView(_scrollPos);
        GUILayout.Label("", GUILayout.MaxHeight(16), GUILayout.MinHeight(16), GUILayout.MinWidth(16), GUILayout.MaxWidth(16));
        if (GUI.Button(GUILayoutUtility.GetLastRect(), "x"))
        {
            Close();
        }
        foreach (CubemapFace thing in System.Enum.GetValues(typeof(CubemapFace)))
        {
            if (thing.SelectedName(false) != "Unknown")
                DrawTextureButton(thing);
        }
        GUILayout.EndScrollView();

    }
    void DrawTextureButton(CubemapFace face)
    {
        if (!_cubemap.isReadable) { Close(); return; }
        var tex = new Texture2D(_cubemap.width, _cubemap.height, TextureFormat.ARGB32, false);
        var pixels = _cubemap.GetPixels(face);
        System.Array.Reverse(pixels);
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Apply();

        GUILayout.Label("", GUILayout.MaxHeight(128), GUILayout.MinHeight(128), GUILayout.MinWidth(128), GUILayout.MaxWidth(128));
        if (GUI.Button(GUILayoutUtility.GetLastRect(), ""))
        {
            CreateTexture(face);
        }
        GUI.DrawTexture(GUILayoutUtility.GetLastRect().ShrinkBy(new(8, 8, 8, 8)), tex, ScaleMode.ScaleToFit, true);
    }
    void CreateTexture(CubemapFace face)
    {
        SetImporterSettings(_importer, _cubemapPath, true, 2048);
        var tex = new Texture2D(_cubemap.width, _cubemap.height, TextureFormat.ARGB32, false);
        var pixels = _cubemap.GetPixels(face);
        System.Array.Reverse(pixels);
        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Bilinear;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Apply();
        Finish(tex, face.SelectedName(true));
    }
    public void Finish(Texture2D tex, string suffix)
    {



        //CreateTexture();


        var parentDirPath = _cubemapPath;

        //set save path
        parentDirPath = _cubemapPath.Split("/" + _cubemap.name)[0];
        //var parentDirName = parentDirPath[(parentDirPath.LastIndexOf("/") + 1)..];
        tex.name = $"{_cubemap.name}-{suffix}";
        //var grandparentPath = parentDirPath.Split("/" + parentDirName)[0];
        //var newParentPath = grandparentPath + "/Thumbs/";
        var texPath = parentDirPath + "/" + tex.name + ".png";

        //save texture
        //if (!Directory.Exists(newParentPath)) { Directory.CreateDirectory(newParentPath); }
        //AssetDatabase.CreateAsset(tex, texPath);
        var bytes = ImageConversion.EncodeToPNG(tex);
        File.WriteAllBytes(Application.dataPath + texPath.Replace("Assets/", "/"), bytes);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.ImportAsset(texPath);
        AssetDatabase.Refresh();
        EditorMethods.ReOpenCurrentScene();

        //restore original importer settings for cubemap
        SetImporterSettings(_importer, _cubemapPath, false, 2048);

        //set texture clamp mode
        _importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        _importer.wrapMode = TextureWrapMode.Mirror;
        _importer.filterMode = FilterMode.Point;
        SetImporterSettings(_importer, texPath, false, 2048);

        Selection.activeObject = null;

        Close();
    }

    static void SetImporterSettings(TextureImporter importer, string path, bool readable, int size)
    {
        importer.isReadable = readable;
        importer.maxTextureSize = size;
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
    }
}