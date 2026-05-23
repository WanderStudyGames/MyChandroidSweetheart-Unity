using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayMode
{
#if UNITY_EDITOR
    [InitializeOnEnterPlayMode]
    private static void EnterPlayMode() { OnEnterPlayMode?.Invoke(); EditorWindow.FocusWindowIfItsOpen<SceneView>(); }
    [InitializeOnLoadMethod]
    private static void ModeChangedSubscribe() { EditorApplication.playModeStateChanged += ModeChanged; }
    private static void ModeChanged(PlayModeStateChange playModeState) { if (playModeState == PlayModeStateChange.ExitingPlayMode) OnLeavePlayMode?.Invoke(); }
#endif
    public static event Action OnEnterPlayMode;
    public static event Action OnLeavePlayMode;
}
