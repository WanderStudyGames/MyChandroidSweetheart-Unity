using UnityEditor;
using UnityEditor.SceneManagement;
public static class EditorMethods
{
    public static void ReOpenCurrentScene()
    {
        AssetDatabase.OpenAsset(GetCurrentSceneAsset());
    }
    public static SceneAsset GetCurrentSceneAsset()
    {
        return AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorSceneManager.GetActiveScene().path);
    }
}