#define QC_DISABLE_BUILTIN_ALL

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using VInspector;

using System.Linq;

using UnityEngine.Rendering;

#if UNITY_EDITOR

using UnityEditor;

#endif

[CreateAssetMenu(fileName = "Scene Handler", menuName = "ScriptableObjects/Scenes/Scene Handler")]
public class SceneHandler : ScriptableObject
{
    [SerializeField] Material[] _blurMaterials;
    [SerializeField] Shader _HQblurShader;
    [SerializeField] Shader _LQblurShader;
    [Header("Demo End Scene")]
    [SerializeField] SceneAssetReference _demoEndScene;
    [SerializeField] SceneAssetReference[] _nonGameScenes;
    private static SceneHandler instance;
    #region Load ScriptableObjects
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void InitGame()
    {
        PlayerPrefs.DeleteKey("Screenmanager Is Fullscreen mode");
        PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
        PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
        Resources.LoadAll<ScriptableObject>("");
#if UNITY_EDITOR
        SaveSystem.Load(0);
#else
        SaveSystem.Load(-1);
#endif
#if PLATFORM_ANDROID

        Shader.EnableKeyword("PLATFORM_ANDROID");

        if (Shader.enabledGlobalKeywords.Any(a => { return a.name == "PLATFORM_ANDROID"; }))
        {
            Debug.LogError("yup");
        }
#else
        Shader.DisableKeyword("PLATFORM_ANDROID");
#endif
    }
    #endregion
    public static event Action<string> BeforeSceneLoad;
    private void OnEnable()
    {
        instance = this;
        PlayMode.OnEnterPlayMode += Init;
        SaveSystem.OnSaveFile += Save;
        SaveSystem.OnLoadFile += Load;
        Init();
    }
    private void Save(SaveSystem.SaveFileNames files)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (instance == null || (_nonGameScenes != null && _nonGameScenes.Any(sc => sc.SceneName == currentScene))) return;
        ES3.Save("sceneName", currentScene, files.worldData);
        ES3.Save("destSpawnPoint", DestSpawnpoint, files.worldData);
        ES3.Save("fadeColor", UIFade.FadeColor, files.worldData);
        ES3.Save("fadeDurations", UIFade.FadeDurations, files.worldData);
    }
    private static string _loadedSceneName = "City01-HomeInterior";
    private static int _loadedDestSpawn = 0;
    private static Color _loadedUIColor = Color.white;
    private static Vector3 _loadedUIFades = Vector3.one * 0.3f;
    private void Load(SaveSystem.SaveFileNames files)
    {
        _loadedSceneName = ES3.Load("sceneName", files.worldData, _loadedSceneName);
        _loadedDestSpawn = ES3.Load("destSpawnPoint", files.worldData, DestSpawnpoint);
        _loadedUIColor = ES3.Load("fadeColor", files.worldData, Color.white);
        _loadedUIFades = ES3.Load("fadeDurations", files.worldData, Vector3.one * 0.3f);
    }
    public static void LoadLoadedScene()
    {
        UIFade.FadeColor = _loadedUIColor;
        UIFade.FadeDurations = _loadedUIFades;
        LoadScene(_loadedSceneName, _loadedDestSpawn);
    }
    [Button]
    void Init()
    {
        var keyword = GlobalKeyword.Create("_DISABLE_OUTLINE");
#if PLATFORM_WEBGL
        Shader.EnableKeyword(keyword);
#else
        Shader.DisableKeyword(keyword);
#endif
        DestSpawnpoint = 0;
        foreach (var material in _blurMaterials)
        {
#if PLATFORM_EXTENDS_VULKAN_DEVICE || PLATFORM_WEBGL || PLATFORM_ANDROID
            material.shader = _LQblurShader;
#else
            material.shader = _HQblurShader;
#endif
#if !UNITY_EDITOR && PLATFORM_ANDROID

            // only support controllers on the console we're interested in. Prohibit mouse and keyboard
            string[] supportedDevices = { "Gamepad", "Joystick", "VirtualMouse", "Touchscreen" };
            UnityEngine.InputSystem.InputSystem.settings.supportedDevices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<string>(supportedDevices);
#endif
        }

    }
    public static int DestSpawnpoint { get; private set; }
    private static bool SceneExists(string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = SceneNameFromPath(scenePath);
            if (sceneName == name)
            {
                return true;
            }
        }
        return false;
    }
    public static void LoadScene(string name, int spawnpoint = 0)
    {
        CompanionData.CurrentSceneLocation = null;
        DestSpawnpoint = spawnpoint;

        if (!SceneExists(name))
        {
            Debug.LogError($"Scene '{name}' does not exist in build settings. Loading demo end scene instead.");
            name = instance._demoEndScene.SceneName;
        }

        BeforeSceneLoad?.Invoke(name);
        SceneManager.LoadScene(name);
    }
    private static string SceneNameFromPath(string path)
    {
        return path.Split("/").Last().Replace(".unity", "");
    }
    public static void LoadScene(int scene, int spawnpoint = 0)
    {
        LoadScene(SceneNameFromPath(SceneUtility.GetScenePathByBuildIndex(scene)), spawnpoint);
    }

    /// <summary>
    /// Only for use with UnityEvents
    /// </summary>
    public static void FadeThenResetScene()
    {
        UIFade.FadeColor = Color.black;
        UIFade.FadeDurations = Vector3.one * 0.3f;
        if (!UIFade.Exists) { ResetScene(); return; }
        UIFade.ExecuteAfterFade(() => { ResetScene(); });
    }
    public static void SetSpawnpoint(int i)
    {
        DestSpawnpoint = i;
    }
    public static void ResetScene()
    {
        LoadScene(SceneManager.GetActiveScene().name, DestSpawnpoint);
    }
    public static void MainMenu()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        return;
#endif
        if (UIFade.Exists)
        {
            UIFade.FadeColor = Color.white;
            UIFade.FadeDurations = Vector3.one;
            UIFade.ExecuteAfterFade(() =>
            {
                SaveSystem.Save();
                LoadScene("MainMenu");
            }, true);
        }

        else { SaveSystem.Save(); LoadScene("MainMenu"); }
    }
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
#if UNITY_EDITOR
    public static void AddSceneToBuildList(SceneAsset asset)
    {
        var scenes = EditorBuildSettings.scenes.ToList();
        var newScene = new EditorBuildSettingsScene();
        newScene.enabled = true;
        newScene.path = AssetDatabase.GetAssetOrScenePath(asset);
        bool found = false;
        foreach (var scene in scenes)
        {
            if (scene.path == newScene.path) { found = true; break; }
        }
        if (!found) { scenes.Add(newScene); EditorBuildSettings.scenes = scenes.ToArray(); }
    }
    [MenuItem("Tools/CleanBuildList")]
    public static void CleanDeletedFromSceneList()
    {
        var scenes = EditorBuildSettings.scenes.ToList();
        for (int i = scenes.Count - 1; i > -1; i--)
        {
            if (scenes[i].path == "")
            {
                scenes.Remove(scenes[i]);
            }
        }
        EditorBuildSettings.scenes = scenes.ToArray();

    }
#endif
}
