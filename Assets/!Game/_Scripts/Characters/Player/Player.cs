using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerLook))]
public class Player : Character, IAttachableCharacter
{
    [field: SerializeField] public PlayerLook PlayerLook { get; private set; }
    [field: SerializeField] public PlayerMove PlayerMove { get; private set; }
    [field: SerializeField] public PlayerJump PlayerJump { get; private set; }

    [field: SerializeField] private PlayerInput _playerInput;

    private PlayerState _stateBeforePause;
    private void OnPaused()
    {
        _stateBeforePause = PlayerStateManager.State;
        Debug.Log(_stateBeforePause);
        PlayerStateManager.SwitchState(PlayerStates.Menu);
    }
    private void OnUnPaused()
    {
        Debug.Log(_stateBeforePause);
        if (_stateBeforePause != null)
        {
            PlayerStateManager.SwitchState(_stateBeforePause);
            _stateBeforePause = null;
        }
        StartCoroutine(Co_WaitAFrame());
        IEnumerator Co_WaitAFrame() { yield return null; Cursor.lockState = CursorLockMode.Locked; }
    }
    public void Die(Sprite sprite, SFX sfx)
    {
        PlayerStateManager.SwitchState(PlayerStates.Default);
        PlayerMove.SetDisableControls(true);
        PlayerJump.SetDisableGravity(true);
        GlobalPlayerInput.Instance.enabled = false;
        DeathUI.SetSprite(sprite);
        UIFade.FadeColor = Color.black;
        UIFade.FadeDurations = new(0, 0, 1);
        MusicManager.KillBGM();
        UIFade.ExecuteAfterFade(() =>
        {
            StartCoroutine(Co_DeathWait());

        });
        IEnumerator Co_DeathWait()
        {
            yield return new WaitForSecondsRealtime(4);
            SceneHandler.ResetScene();
            PlayerMove.CharacterControllerMove.SetVelocity(Vector3.zero);
            yield break;
        }
        if (sfx != null)
            DontDestroyOnLoad(sfx.PlayAtPoint(transform.position));
    }
    private void OnEnable()
    {
        PauseMenu.OnOpen += OnPaused;
        PauseMenu.OnClose += OnUnPaused;
    }
    private void OnDisable()
    {
        PauseMenu.OnOpen -= OnPaused;
        PauseMenu.OnClose -= OnUnPaused;
    }
    private float _bottomlessPitHeight;
    void Awake()
    {
        QualitySettings.vSyncCount = 1;
        PlayerLook = GetComponent<PlayerLook>();
        _bottomlessPitHeight = (SceneStartup.Instance != null) ? SceneStartup.Instance.BottomlessPitHeight : -50f;
    }
    [ContextMenu("Player/Die")]
    private void Die()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.AddComponent<Rigidbody>().AddForce(transform.forward * 4f);
        PlayerLook.enabled = false;
    }
    private bool teleporting = false;
    private void Update()
    {
        if (teleporting) return;
        if (transform.position.y < _bottomlessPitHeight)
        {
            PlayerManager.TeleportToLastGroundPosition();
            teleporting = true;
        }
    }

    public void TeleportToClosest(Transform[] transforms)
    {
    }
}
