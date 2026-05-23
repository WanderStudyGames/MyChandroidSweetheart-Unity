using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SignalOutput : DeviceComponent, ISignalReceivable
{
    [Dependency][SerializeField] private List<SignalInput> signalInputs = new();
    [SerializeField] private bool _sendEventsWhenDisabled = false;
    public List<SignalInput> SignalInputs => signalInputs;
    public static bool drawLines = true;

    public bool onner { get; private set; }

    public void CallbackForInputs(Action<SignalInput> action)
    {
        if (!enabled && !_sendEventsWhenDisabled) return;
        foreach (SignalInput input in signalInputs)
        {
            if (input == null) continue;
            action(input);
        }
    }
    private void Start()
    {

    }

    public override void On()
    {
        if (onner) return;
        onner = true;
        CallbackForInputs(input => { input.On(); });
    }
    public override void Off()
    {
        if (!onner) return;
        onner = false;
        CallbackForInputs(input => { input.Off(); });
    }
    public override void SingleClick()
    {
        CallbackForInputs(input => { input.SingleClick(); });
    }
    public override void Int(int i)
    {
        CallbackForInputs(input => { input.Int(i); });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (SignalInput input in signalInputs)
        {
            if (input == null || !drawLines) continue;
            Gizmos.DrawLine(transform.position, input.gameObject.transform.position);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (SignalInput input in signalInputs)
        {
            if (input == null) continue;
            Gizmos.DrawLine(transform.position, input.gameObject.transform.position);
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(SignalOutput))]
class SignalOutputEditor : Editor
{
    private void OnSceneGUI()
    {
        SignalInput[] signalInputs = FindObjectsOfType<SignalInput>();

        var sOut = target as SignalOutput;
        var sGO = sOut.gameObject;
        signalInputs = signalInputs.Where(input => input.gameObject != sGO).ToArray();

        var sInp = sOut.SignalInputs;
        var rot = SceneView.lastActiveSceneView.camera.transform.rotation;
        if (sGO == Selection.activeObject)
        {
            foreach (SignalInput input in signalInputs)
            {
                var pos = input.transform.position;
                var size = 1f;
                if (input == null) continue;
                Handles.color = Color.red;
                if (sInp.Contains(input) && Handles.Button(pos, rot, size, size, Handles.CircleHandleCap))
                {
                    Undo.RecordObject(sOut, "Disconnect Signal Output");
                    //sOut.SignalInputs.Remove(input);
                    sInp.Remove(input);
                    SignalListCleanup(sInp);
                    EditorUtility.SetDirty(sOut);
                }
                Handles.color = Color.yellow;
                if (!sInp.Contains(input) && Handles.Button(pos, rot, size, size, Handles.RectangleHandleCap))
                {
                    Undo.RecordObject(sOut, "Connect Signal Output");
                    sInp.Add(input);
                    SignalListCleanup(sInp);
                    //if (input.EventCount <= sOut.IntMax) { Logger.Warning($"{input.gameObject.name}.{nameof(SignalInput)} event count is lower than {nameof(sOut.IntMax)} on {sGO.name}.{nameof(SignalOutput)}"); }
                    EditorUtility.SetDirty(sOut);
                }
            }

        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var sOut = target as SignalOutput;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Draw Lines");
        SignalOutput.drawLines = EditorGUILayout.Toggle(SignalOutput.drawLines);
        EditorGUILayout.EndHorizontal();
    }
    static void SignalListCleanup(List<SignalInput> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null) list.Remove(list[i]);
        }
    }
}
#endif