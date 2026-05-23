using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutsceneTransition : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField, Tooltip("Locks player movement during the fade.")] private bool _lockPlayerMovement = true;
    [SerializeField] private Vector3 _uiFadeTimes = Vector3.one * 0.3f;
    [SerializeField] private Color _uiFadeColor = Color.black;
    [SerializeField] private UnityEvent _onStartCutscene;
    [SerializeField] private UnityEvent _onEndCutscene;
    public void StartCutscene()
    {
        if (_lockPlayerMovement)
        {
            PlayerManager.SetCameraMovementEnabled(false);
            PlayerManager.SetMovementEnabled(false);
            PlayerManager.SetGravityEnabled(false);
        }

        UIFade.FadeColor = _uiFadeColor;
        UIFade.FadeDurations = _uiFadeTimes;

        if (UIFade.Exists)
        {
            UIFade.ExecuteAfterFade(() =>
            {
                PlayerManager.SetPlayerEnabled(false);
                _playableDirector.Play();
                _onStartCutscene.Invoke();
            }, false);
        }
        else
        {
            Debug.LogError("UIFade did not exist.");
            PlayerManager.SetPlayerEnabled(false);
            _playableDirector.Play();
            _onStartCutscene.Invoke();
        }
    }
    public void EndCutscene(Transform spawnpoint)
    {
        _playableDirector.Stop();
        PlayerManager.SpawnPlayer(spawnpoint.position, spawnpoint.rotation);
        UIManager.SpawnUI();
        _onEndCutscene?.Invoke();

    }
}
