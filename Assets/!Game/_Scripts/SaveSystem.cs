using QFSW.QC;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

[CreateAssetMenu(fileName = "SaveSystem", menuName = "ScriptableObjects/SaveSystem")]
public class SaveSystem : ScriptableObject
{
    public static bool IsDemo() => Application.version.ToLower().Contains("demo");

    public static bool IsKiosk { get; private set; } = false;
    public static void SetKiosk(bool kiosk) { IsKiosk = kiosk; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Init()
    {
        IsKiosk = ES3.Load("kioskMode", "staticData.es3", IsDemo());
    }
    [Command("savefile-activated")]
    public static bool CanSave { get; private set; } = true;
    public static void ActivateSaves()
    {
        Debug.Log("Saves activated");
        CanSave = true;
    }
    public static void DeactivateSaves()
    {
        Debug.Log("Saves deactivated");
        CanSave = false;
    }

    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Load;
        PlayMode.OnLeavePlayMode += Save;
#if !UNITY_EDITOR
        Application.quitting += Save;
#endif
    }
    public static event Action<SaveFileNames> OnSaveFile;
    public static event Action<SaveFileNames> OnLoadFile;
    public static event Action OnSaveStatic;
    public static event Action OnLoadStatic;

    public struct SaveFileNames
    {
        public string materialData;
        public string companionData;
        public string playerData;
        public string worldData;
    }

    public static SaveFileNames Files { get; private set; } = new()
    {
        companionData = "companionData0.es3",
        playerData = "playerData0.es3",
        materialData = "materialData0.es3",
        worldData = "worldData0.es3"
    };
    public static int SaveFile { get; private set; } = -1;
    public static void Load(int file)
    {
        OnLoadStatic?.Invoke();
        if (!CanSave) return;
        if (file != -1)
            file = Mathf.Clamp(file, 0, 5);
        SaveFile = file;
        OnLoadFile?.Invoke(Files);
    }
    public static void LoadEmptyFile()
    {
        SaveFile = 0;
        Files = new();
        OnLoadFile?.Invoke(Files);
    }
    [Button]
    private static void Load()
    {
        if (!CanSave) return;
        OnLoadFile?.Invoke(Files);
    }
    [Button]
    [Command("savefile-save")]
    public static void Save()
    {
        Save(SaveFile);
    }
    [Command("savefile-save")]
    public static void Save(int file)
    {
        OnSaveStatic?.Invoke();
        ES3.Save("kioskMode", IsKiosk, "staticData.es3");

        if (!CanSave)
        {
            Debug.LogWarning("SaveSystem is deactivated, cannot save.");
            return;
        }
        if (SaveFile == -1) return;
        Debug.Log($"Saving to file {file}");

        SaveFile = Mathf.Clamp(file, 0, 5);
        SetFiles();

        if (SceneManager.GetActiveScene().name != "MainMenu")
            OnSaveFile?.Invoke(Files);
    }
    [Command("savefile-delete")]
    public static void Delete(int file)
    {
        if (!CanSave) return;
        file = Mathf.Clamp(file, 0, 5);
        ES3.DeleteFile(filePath: $"playerData{file}.es3");
        ES3.DeleteFile(filePath: $"companionData{file}.es3");
        ES3.DeleteFile(filePath: $"materialData{file}.es3");
        ES3.DeleteFile(filePath: $"worldData{file}.es3");
    }
    [Command("savefile-load")]
    private static void HotLoad(int fileNumber)
    {
        if (!CanSave) return;
        Load(fileNumber);
        SceneHandler.ResetScene();
    }

    public static void SaveSnapshot(SaveFileNames files)
    {
        OnSaveFile?.Invoke(files);
    }

    public static void LoadSnapshot(SaveFileNames files)
    {
        OnLoadFile?.Invoke(files);
    }
    private static void SetFiles()
    {
        Files = new()
        {
            companionData = $"companionData{SaveFile}.es3",
            materialData = $"materialData{SaveFile}.es3",
            playerData = $"playerData{SaveFile}.es3",
            worldData = $"worldData{SaveFile}.es3"
        };
    }
}
