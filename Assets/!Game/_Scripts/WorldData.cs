using UnityEngine;
using QFSW.QC;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static partial class WorldData
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void OnInit()
    {
        WorldFlags.OnDataModified += SaveNotification.Show;
    }
    public static SavedDataList<string> CollectedCoins { get; private set; } = new("collectedCoins", files => files.worldData);
    public static SavedDataList<string> WorldFlags { get; private set; } = new("worldFlags", files => files.worldData);
    public static SavedDataList<string> CompletedScans { get; private set; } = new("completedScans", files => files.worldData);
    public static SavedDataList<string> DiscoveredRooms { get; private set; } = new("discoveredRooms", files => files.worldData);
    public static SavedDataList<string> SceneBools { get; private set; } = new("sceneBools", files => files.worldData);
    public static SavedDataList<string> StaticFlags { get; private set; } = new("staticFlags", files => { return "staticData.es3"; }, isStatic: true);

    [Command("worldflag-list")]
    private static string[] GetWorldFlagsCommand() { return WorldFlags.GetValues(); }
    [Command("worldflag-add")]
    private static string AddWorldFlagCommand(string flag)
    {
        WorldFlags.Add(flag);
        return $"Flag {flag} added.";
    }
    [Command("worldflag-clear")]
    private static string ClearWorldFlagsCommand()
    {
        WorldFlags.Clear();
        return "Cleared world flags.";
    }
    [Command("worldflag-remove")]
    private static string RemoveWorldFlagCommand([WorldFlagTag] string flag)
    {
        WorldFlags.Remove(flag);
        return $"Removed world flag {flag}.";
    }
    [Command("scenebool-add")]
    private static string AddSceneBoolCommand(string flag)
    {
        SceneBools.Add(flag);
        return $"Scene bool {flag} added.";
    }
    [Command("scenebool-clear")]
    private static string ClearSceneBoolsCommand()
    {
        SceneBools.Clear();
        return "Cleared scene bools.";
    }
    [Command("scenebool-remove")]
    private static string RemoveSceneBoolCommand([SceneBoolTag] string flag)
    {
        SceneBools.Remove(flag);
        return $"Removed scene bool {flag}.";
    }
}
