using System.Collections;
using UnityEditor;

[EditorWindowTitle(title = "Scenes")]
public class SceneButtonWindow : AssetButtonWindow<SceneAsset>
{
    protected override string Query => "t:SceneAsset " + query;
    [MenuItem("Window/Scene Viewer")]
    private static void Enable()
    {
        EditorWindow.GetWindow<SceneButtonWindow>("Scenes");
    }
}
