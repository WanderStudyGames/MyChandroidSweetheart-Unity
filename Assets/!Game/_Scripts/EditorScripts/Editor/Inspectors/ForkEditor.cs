using UnityEditor;

//[CustomEditor(typeof(Fork))]
public class ForkEditor : Editor
{
    private Fork _fork;
    private void OnEnable()
    {
        _fork = (Fork)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Condition"));
        switch (_fork.Condition)
        {
            case Fork.Conditions.WorldFlag:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_worldFlag"));
                break;
            case Fork.Conditions.SceneBool:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_sceneBool"));
                break;
            case Fork.Conditions.InventoryItem:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_inventory"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_inventoryItem"));
                break;
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_false"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_true"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_executeOnAwake"));
        serializedObject.ApplyModifiedProperties();
    }
}
