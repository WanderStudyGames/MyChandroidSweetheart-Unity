using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//[CustomEditor(typeof(DialogueSequence))]
public class DialogueSequenceEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var container = new VisualElement();
        container.Add(new PropertyField(serializedObject.FindProperty("_sequenceEntries")));
        return container;
    }

}
