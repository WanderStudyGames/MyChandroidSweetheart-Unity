using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class BatterySceneLoader : SceneLoader
{
    [Dependency][SerializeField] private PostProcessProfile _profile;
    [Dependency][SerializeField] private AudioMixerSnapshot _snapshot;
    [SerializeField] private InteractibleObject _interactibleObject;
    [SerializeField] private UnityEvent _onAwakeAndSolved;
    public string SceneName => SceneAssetReference.SceneName;
    private void Awake()
    {
        if (_interactibleObject != null && !Inventories.Instance.PlayerInventory.Has("QuanTrav Device")) _interactibleObject.SetCanInteract(false);
        if (WorldData.SceneBools.Has(SceneName))
            _onAwakeAndSolved.Invoke();


    }
    private void Start()
    {
        AudioSnapshotManager.DefaultSnapshot.TransitionTo(2f);
    }
    public override void LoadScene()
    {
        MusicManager.KillBGM();
        if (_snapshot != null)
            _snapshot.TransitionTo(2f);
        UIFade.FadeDurations = _fadeTime;
        UIFade.FadeColor = FadeColor;
        if (_profile != null)
        {
            UIFade.ExecuteAfterFade(null);
            PostProcProfileController.ExecuteAfterTransition(() =>
            {
                UIFade.LoadingEnabled(true);
                SceneHandler.LoadScene(SceneName, _spawnpointToLoad);
            }, _profile, _fadeTime);
        }
        else
        {
            UIFade.ExecuteAfterFade(() =>
            {
                UIFade.LoadingEnabled(true);
                SceneHandler.LoadScene(SceneName, _spawnpointToLoad);
            });
        }
    }
}
