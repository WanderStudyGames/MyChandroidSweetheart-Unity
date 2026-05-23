using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BatterySceneLoader))]
class BatterySceneLoaderEditor : Editor
{
    private void OnSceneGUI()
    {
        var loader = (BatterySceneLoader)target;
        var transform = target.GameObject().transform;
        var sceneAssetReference = loader.SceneAssetReference;
        var pos = transform.position + (Vector3.down * 2);
        var size = HandleUtility.GetHandleSize(pos) / 2;
        if (Handles.Button(pos, transform.rotation, size, size, Handles.ConeHandleCap))
        {
            SceneLoader.PreviousScene = sceneAssetReference;
            AssetDatabase.OpenAsset(sceneAssetReference.Asset);
        };
    }
}