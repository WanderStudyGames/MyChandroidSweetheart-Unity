using QFSW.QC;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Companion Data", menuName = "ScriptableObjects/Companion/Companion Data")]
public class CompanionData : ScriptableObject
{
    public static Vector3 LastKnownSpawnPosition { get; private set; }
    public static Vector3 Position { get; set; }
    public static void RecordPosition(Vector3 p) { LastKnownSpawnPosition = p; }
    public static string CurrentSceneLocation { get; set; }

    public bool BustPhysics { get; private set; }
    public void SetBustPhysics(bool b)
    {
        if (b != BustPhysics)
        {
            BustPhysics = b;
            OnBustPhysicsChanged?.Invoke();
        }
    }
    public static event Action OnBustPhysicsChanged;

    public static Vector2 LocalVelocity { get; set; }
    public static string SceneGoingTo { get; set; }
    public static Command CurrentCommand { get; private set; }

    public static void Command() { CurrentCommand = null; }
    public static void Command(Transform tr) { CurrentCommand = new(tr); }
    public static void Command(Vector3 v3) { CurrentCommand = new(v3); }
    [Command("companion-name")]
    public static string CompanionName { get; private set; } = "Rosie";
    public static void SetCompanionName(string name) { if (!string.IsNullOrEmpty(name)) { CompanionName = name; } }
    private void Load(SaveSystem.SaveFileNames files)
    {
        CompanionName = ES3.Load("name", files.companionData, "Rosie");
    }
    private void Init() { BustPhysics = ES3.Load("bustPhysics", "settings.es3", true); }
    private void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save("name", CompanionName, files.companionData);
        ES3.Save("bustPhysics", BustPhysics, "settings.es3");
    }
    private void OnEnable()
    {
        LastKnownSpawnPosition = Vector3.zero;
        SaveSystem.OnSaveFile += Save;
        SaveSystem.OnLoadFile += Load;
        SceneGoingTo = null;
        Init();
        Load(SaveSystem.Files);
    }
}
public class Command
{
    public Vector3 Vector3 { get; private set; }
    public Transform Transform { get; private set; }
    public Command(Vector3 v3)
    {
        v3.QuantizeToNavmesh(out Vector3 q);
        Vector3 = q;
        Transform = null;
    }
    public Command(Transform tr)
    {
        Transform = tr;
        Vector3 = Vector3.zero;
    }

}