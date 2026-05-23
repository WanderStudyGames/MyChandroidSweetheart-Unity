using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartup : MonoBehaviour
{
    public static event Action OnSceneAwake;
    public static event Action OnSceneStart;
    public static SceneStartup Instance { get; private set; }
    [SerializeField] private Transform[] spawnpoints;
    [SerializeField] private BGM BGM;
    [SerializeField] private float bottomlessPitHeight = -50f;
    [SerializeField] private bool _spawnPlayerOnAwake = true;
    public float BottomlessPitHeight => bottomlessPitHeight;
    public Transform[] Spawnpoints => spawnpoints;
    public void AddSpawnpoint(Transform t)
    {
        var list = spawnpoints.ToList();
        list.AddUnique(t);
        spawnpoints = list.ToArray();
    }
    public void RemoveSpawnpoint(Transform t)
    {
        var list = spawnpoints.ToList();
        list.RemoveAll(t);
        spawnpoints = list.ToArray();
    }
    private void Awake()
    {
        Debug.Log("AWAKE ON SCENE STARTUP");
        Debug.Log(Resources.Load<ScriptableObject>("Scene Handler"));

        Instance = this;
        PurgeOthers();

        OnSceneAwake?.Invoke();

        MusicManager.PlayBGM(BGM);
    }
    private void Start()
    {
        OnSceneStart?.Invoke();
        if (_spawnPlayerOnAwake)
        {
            SpawnPlayer();
        }
        Resources.UnloadUnusedAssets();
    }
    public void SpawnPlayer()
    {
        if (Instance == null) Instance = this;
        if (Instance.spawnpoints.Length == 0)
        {
            Logger.Warning("Spawnpoint List is empty. Player won't spawn properly.");
        }
        CompanionSpawnTrigger.ResetSpawnFlag();
        PlayerManager.SpawnPlayer(this);
        UIManager.SpawnUI();
        WorldData.DiscoveredRooms.Add(SceneManager.GetActiveScene().name);
    }
    public void SpawnPlayer(int spawnpointIndex)
    {
        SceneHandler.SetSpawnpoint(spawnpointIndex);
        SpawnPlayer();
    }
    public void SpawnPlayer(Transform location)
    {
        CompanionSpawnTrigger.ResetSpawnFlag();
        PlayerManager.SpawnPlayer(location.position, location.rotation);
        UIManager.SpawnUI();
        WorldData.DiscoveredRooms.Add(SceneManager.GetActiveScene().name);
    }
    public void SpawnPlayer(Spawnpoint spawnpoint)
    {
        for (int i = 0; i < Spawnpoints.Length; i++)
        {
            if (Spawnpoints[i] == spawnpoint.transform)
            {
                SceneHandler.SetSpawnpoint(i);
                break;
            }
        }
        SpawnPlayer(spawnpoint.transform);
    }
    private void PurgeOthers()
    {
        //ensure only one scene startup exists in each scene
        List<SceneStartup> sceneStartups = FindObjectsOfType<SceneStartup>().ToList();
        sceneStartups.Remove(this);
        if (sceneStartups.Count > 0) { Logger.LogSigned(GetType().Name, $"Found {sceneStartups.Count} duplicates of {GetType().Name}. Purging", new(color: "grey")); }
        foreach (SceneStartup sceneStartup in sceneStartups)
        {
            GameObject.Destroy(sceneStartup);
            Destroy(sceneStartup);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        for (int i = 0; i < Spawnpoints.Length; i++)
        {
            if (Spawnpoints[i] != null)
            {
                var style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = Color.blue;
                style.fontSize = 30;
                Handles.Label(Spawnpoints[i].position + Vector3.up * 1.5f, new GUIContent(i.ToString()), style);
                style.fontSize = 12;
                style.alignment = TextAnchor.UpperCenter;
                Handles.Label(Spawnpoints[i].position, new GUIContent(Spawnpoints[i].gameObject.name), style);
            }
        }

    }
#endif
}
