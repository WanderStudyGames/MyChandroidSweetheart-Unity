using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLoader))]
class SceneLoaderEditor : Editor
{
    private void OnSceneGUI()
    {
        var loader = (SceneLoader)target;
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
[CustomPropertyDrawer(typeof(SceneAssetReference))]
class SceneAssetReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var sceneName = property.FindPropertyRelative("_sceneName").stringValue;
        var sceneAsset = property.FindPropertyRelative("_asset");
        if (sceneName == "") sceneName = "Empty";
        EditorGUI.PropertyField(position, sceneAsset, new GUIContent(sceneName));
    }
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 10;
}