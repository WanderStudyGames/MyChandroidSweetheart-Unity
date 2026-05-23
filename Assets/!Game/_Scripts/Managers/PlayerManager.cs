using QFSW.QC;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Manager", menuName = "ScriptableObjects/Player/Player Manager")]
public class PlayerManager : ScriptableObject
{
    private static PlayerManager instance;
    [Dependency][SerializeField] private Player _prefabField;
    private static GameObject _prefab => instance._prefabField.gameObject;
    public static string Tag => _prefab.tag;
    private GameObject _goInstance;
    private Player _playerInstance;

    private bool loadObjectPosition;
    public static event Action<bool> OnSetPlayerEnabled;
    public static void SetPlayerEnabled(bool b)
    {
        if (instance == null) return;
        if (instance._goInstance == null) return;
        UIManager.SetUIEnabled(b);
        instance._goInstance.SetActive(b);
        OnSetPlayerEnabled?.Invoke(b);
    }

    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Init;
        SceneHandler.BeforeSceneLoad += ResetCarriedItem;

        Init();
    }
    private void Init()
    {
        instance = this;
        _goInstance = null;
        _playerInstance = null;
        loadObjectPosition = FindObjectOfType<SceneStartup>() != null;
    }
    public static void SetMovementEnabled(bool enabled)
    {
        if (instance._playerInstance == null) return;
        if (instance._playerInstance.PlayerMove == null) Debug.Log("FUUUUUUUCK");
        instance.
            _playerInstance.
            PlayerMove.
            enabled = enabled;
        instance._playerInstance.PlayerJump.enabled = enabled;
    }
    public static void SetCameraMovementEnabled(bool enabled)
    {
        if (instance._playerInstance == null) return;
        instance._playerInstance.PlayerLook.enabled = enabled;
    }
    public static void SetGravityEnabled(bool enabled)
    {
        if (instance._playerInstance == null) return;
        instance._playerInstance.PlayerJump.SetDisableGravity(!enabled);
    }
    public static void SetCommandsEnabled(bool enabled)
    {
        instance._goInstance.GetComponent<Commands>().enabled = enabled;
    }
    public static void Die(Sprite sprite)
    {
        if (instance._goInstance == null) return;
        if (instance._playerInstance == null) return;
        instance._playerInstance.Die(sprite, null);
    }
    public static void SetInteractEnabled(bool enabled)
    {
        instance._goInstance.GetComponent<Interact>().enabled = enabled;
    }
    public static void SetCameraEnabled(bool enabled)
    {
        if (instance._goInstance == null) return;
        instance._goInstance.GetComponent<PlayerLook>().GetCamera().enabled = enabled;
    }
    public static void SpawnPlayer(Vector3 pos, Quaternion rot)
    {
        instance._goInstance = GameObjectUtility.SpawnUnique(pos, rot, instance._prefabField.gameObject);
        instance._playerInstance = instance._goInstance.GetComponent<Player>();

        PlayerAbilityManager.GameObjectInstance = instance._goInstance;
        PlayerAbilityManager.RefreshComponents();
        instance.loadObjectPosition = false;
    }
    public static void Jitter(float duration)
    {
        if (instance._goInstance == null) return;
        var jitter = instance._goInstance.GetComponentInChildren<Jitter>();
        if (jitter != null) jitter.StartJitterTimer(duration);
    }
    public static void CommandFollow()
    {
        if (instance._goInstance == null) return;
        CompanionData.Command(instance._goInstance.transform);

    }
    public static void SpawnPlayer(SceneStartup sceneStartup)
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        if (sceneStartup.Spawnpoints.Length > 0)
        {
            if (SceneHandler.DestSpawnpoint > -1 && SceneHandler.DestSpawnpoint < sceneStartup.Spawnpoints.Length)
            {
                pos = sceneStartup.Spawnpoints[SceneHandler.DestSpawnpoint].position;
                rot = sceneStartup.Spawnpoints[SceneHandler.DestSpawnpoint].rotation;
                //sceneStartup.Spawnpoints[SceneHandler.DestSpawnpoint].GetComponent<Spawnpoint>().ExecuteSpawnEvent();
            }
        }
#if UNITY_EDITOR
        if (instance.loadObjectPosition && GameObjectUtility.TryGetObjectTransform("PlayerDebugSpawn", out Transform transform))
        {
            pos = transform.position;
            rot = transform.rotation;
            //Destroy(transform.gameObject);
        }
#endif
        SpawnPlayer(pos, rot);
    }
    #region move_player
    [Command("player-die")]
    public static void PlayerDie()
    {
        Die(null, null);
    }
    public static void Die(Sprite sprite, SFX sfx)
    {
        if (instance._goInstance == null) return;
        if (instance._playerInstance == null) return;
        instance._playerInstance.Die(sprite, sfx);
    }
    public static void TeleportWithFade(Transform tr)
    {
        TeleportWithFade(tr.position, tr.rotation);
    }
    public static void Teleport(Transform tr)
    {
        Teleport(tr.position, tr.rotation);
    }
    public static void TeleportToLastGroundPosition()
    {
        TeleportWithFade(PlayerData.LastGroundLocation, PlayerData.Rotation);
    }
    public static void TeleportWithFade(Vector3 pos, Quaternion rot)
    {
        UIFade.FadeColor = Color.white;
        UIFade.FadeDurations = Vector3.one * 0.2f;
        UIFade.ExecuteAfterFade(() =>
        {
            instance._goInstance = GameObjectUtility.SpawnUnique(pos, rot, _prefab);
            PlayerAbilityManager.GameObjectInstance = instance._goInstance;
            PlayerAbilityManager.RefreshComponents();
            ResetCarriedItem();
            UIFade.FadeIn();
        }, false);
    }
    public static void Teleport(Vector3 pos, Quaternion rot)
    {
        instance._goInstance = GameObjectUtility.SpawnUnique(pos, rot, _prefab);
        instance._playerInstance = instance._goInstance.GetComponent<Player>();
        PlayerAbilityManager.GameObjectInstance = instance._goInstance;
        PlayerAbilityManager.RefreshComponents();
        ResetCarriedItem();
        UIFade.FadeIn();
    }
    #endregion
    public static void ResetCarriedItem(string sceneName = null)
    {
        //if (PlayerData.CarriedObject != null)
        //    PlayerData.CarriedObject.Drop(PlayerData.CarriedObject.gameObject.transform.position);
    }

    [ContextMenu("Refresh Componentns")]
    public void Refresh() { PlayerAbilityManager.RefreshComponents(); }

    #region dialogue
    public static void SetDialogueRestrictions(Transform focusObject, float lerpSpeed = 50f)
    {
        if (focusObject != null)
        {
            PlayerStateManager.SwitchState(PlayerStates.DialogueFrozen);
        }
        instance._goInstance.GetComponent<PlayerLook>().LerpTowardsObject(focusObject, lerpSpeed);
    }//
    #endregion
}