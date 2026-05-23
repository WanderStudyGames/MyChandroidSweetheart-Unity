using QFSW.QC;
using UnityEditor;
using UnityEngine;

public class SetPlayerName : MonoBehaviour
{
    [Command("player-name")]
    public static string PlayerName { get; private set; } = "Shorty";
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#endif
    [RuntimeInitializeOnLoadMethod]
    private static void OnInit()
    {
        SaveSystem.OnLoadFile += Load;
        SaveSystem.OnSaveFile += Save;
        Load(SaveSystem.Files);
    }
    static void Load(SaveSystem.SaveFileNames files)
    {
        PlayerName = ES3.Load("name", files.playerData, "Shorty");
    }
    static void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save("name", PlayerName, files.playerData);
    }
    public void SetName(string s)
    {
        if (s == "") return;
        PlayerName = s;

    }
}
