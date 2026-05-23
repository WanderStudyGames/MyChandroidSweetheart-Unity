using UnityEngine;
//[CustomEditor(typeof(Lift))]

public class LiftEditor : ExtendedEditor
{
    Texture _icon;
    private void OnEnable()
    {
        _icon = Resources.Load<Texture>("Ico_Lift");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (serializedObject.isEditingMultipleObjects)
        {
            base.OnInspectorGUI();
            return;
        }

        DrawTitleBlock(_icon, "Lift Editor");

        DrawAllProperties();

        serializedObject.ApplyModifiedProperties();
    }
    //private void OnSceneGUI()
    //{
    //    Lift lift = (Lift)target;
    //    if (lift == null) return;

    //    Undo.RecordObject(target, target.name);
    //    for (int i = 0; i < lift.Positions.Length; i++)
    //    {
    //        lift.Positions[i] = Handles.PositionHandle(lift.transform.position + lift.Positions[i], Quaternion.identity) - lift.transform.position;
    //    }

    //}
}