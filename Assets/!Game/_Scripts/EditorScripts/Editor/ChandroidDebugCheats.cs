using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class ChandroidDebugCheats : MonoBehaviour
{

    [MenuItem("Chandroid/Main Menu")]
    public static void MainMenu()
    {
        SceneHandler.LoadScene("MainMenu");
    }
    [MenuItem("Chandroid/Reset Scene")]
    public static void ResetScene()
    {
        UIFade.FadeColor = Color.white;
        UIFade.FadeDurations = Vector2.one * 0.3f;
        UIFade.ExecuteAfterFade(() => { SceneHandler.LoadScene(SceneManager.GetActiveScene().name, 0); });
    }
    [MenuItem("Chandroid/Spawn Companion At Player")]
    public static void SpawnCompanionAtPlayer()
    {
        CompanionManager.Spawn(PlayerData.Position, PlayerData.Rotation);
        CompanionManager.SetInteractObject(CompanionManager.DefaultInteractObject);
    }
    [MenuItem("Chandroid/Fix Modified Probuilder Meshes")]
    public static void FindModifiedProbuilderMeshes()
    {

        var meshes = FindObjectsOfType<ProBuilderMesh>();
        Debug.Log(meshes.Length);
        int i = 0;
        EditorApplication.update += Update;
        void Update()
        {
            if (i >= meshes.Length) { EditorApplication.update -= Update; Debug.Log("Done Fixing ProBuilder Meshes!"); return; }
            var m = meshes[i];
            if (PrefabUtility.IsPartOfAnyPrefab(m) && PrefabUtility.GetPropertyModifications(m).Length > 0)
            {
                PrefabUtility.RevertObjectOverride(m, InteractionMode.UserAction);
                if (m.TryGetComponent(out MeshRenderer mr))
                {
                    mr.enabled = !mr.enabled;
                    mr.enabled = !mr.enabled;
                    Selection.activeObject = mr.gameObject;
                }
            }
            i++;
        }
    }

    [MenuItem("Chandroid/Recompile")]
    private static void Recompile()
    {
        string[] rebuildSymbols = { "RebuildToggle1", "RebuildToggle2" };
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup);

        if (definesString.Contains(rebuildSymbols[0]))
        {
            definesString = definesString.Replace(rebuildSymbols[0], rebuildSymbols[1]);
        }
        else if (definesString.Contains(rebuildSymbols[1]))
        {
            definesString = definesString.Replace(rebuildSymbols[1], rebuildSymbols[0]);
        }
        else
        {
            definesString += ";" + rebuildSymbols[0];
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            definesString);

    }

}
