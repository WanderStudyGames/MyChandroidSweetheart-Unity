using QFSW.QC;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Companion Tracker", menuName = "ScriptableObjects/Companion/Companion Tracker")]
public class CompanionManager : ScriptableObject
{
    private static CompanionManager instance;
    [Dependency][SerializeField] private GameObject _prefab;
    [SerializeField] private DialogueComponents _dialogueComponents;
    public static GameObject Prefab => instance._prefab;
    public static string Tag => Prefab.tag;
    private static InteractibleObject _interactSphere;

    [SerializeField] private CompanionBehaviors _companionBehaviors;
    public static CompanionBehaviors CompanionBehaviors => instance._companionBehaviors;
    private static GameObject _goInstance;
    public GameObject CurrentGO => _goInstance;

    public static bool CompanionGOExists => GameObjectUtility.GameObjectExists(Tag);
    private static bool loadObjectPosition;

    public static bool CompanionEnabled => _goInstance != null && _goInstance.activeInHierarchy;

    private static CompanionBehaviorManager _companionBehaviorManager;
    [Command("companion-with-player")]
    public static bool CompanionWithPlayer { get; set; } = true;

    public static Transform TabletCameraTarget { get; private set; }
    private void OnEnable()
    {
        PlayMode.OnEnterPlayMode += Init;
        SaveSystem.OnLoadFile += Load;
        SceneStartup.OnSceneAwake += OnSceneAwake;
        SaveSystem.OnSaveFile += Save;
        instance = this;
        Load(SaveSystem.Files);
    }
    void Init()
    {
        CompanionData.CurrentSceneLocation = null;
        loadObjectPosition = FindObjectOfType<SceneStartup>() != null;
        CompanionData.Command(null);
    }
    void Load(SaveSystem.SaveFileNames files)
    {
        CompanionWithPlayer = ES3.Load("withPlayer", filePath: files.companionData, defaultValue: true);
    }
    private void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save("withPlayer", CompanionWithPlayer, filePath: files.companionData);
    }
    public static void StandInPlace()
    {
        if (_goInstance != null)
        {
            Debug.LogWarning("COMPANION GO INSTANCE EXISTS");
            CompanionData.Command(_goInstance.transform.position);
        }
        if (_companionBehaviorManager != null)
        {
            Debug.LogWarning("COMPANION BEHAVIOR MANAGER EXISTS");
            _companionBehaviorManager.SetBehavior(CompanionBehaviors.IdleBehavior);
        }
    }
    public static void StandAtLocation(Transform transform)
    {
        if (_goInstance != null)
        {
            IdleCompanionBehavior.RotationTarget = transform;
            CompanionData.Command(transform.position);
        }
        if (_companionBehaviorManager != null)
        {
            _companionBehaviorManager.SetBehavior(CompanionBehaviors.IdleBehavior);
            _companionBehaviorManager.NavMeshAgent.isStopped = false;
        }
    }

    public static void Spawn(Transform transform)
    {
        _goInstance = Spawn(transform.position, transform.rotation);
        SetInteractObject(DefaultInteractObject);
    }

    public static void Scare()
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.Scare();
    }
    public static void Sit()
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.SetBehavior(CompanionBehaviors.SitBehavior);
    }

    public static void FollowPlayer()
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.SetBehavior(CompanionBehaviors.FollowBehavior);
    }

    public static void DropCarriedItem()
    {
        _goInstance.GetComponent<Companion>().DropCarriedItem();
    }
    public static void CarryCompanion()
    {
        _goInstance.GetComponent<PlayerCarryableObject>().Carry();
    }

    public static void HandsUp()
    {

    }
    public static void SetCompanionEnabled(bool enabled)
    {
        if (_goInstance != null)
            _goInstance.SetActive(enabled);
    }
    public static void SetAnimationTrigger(string triggerName)
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.SetAnimatorParam.SetTrigger(triggerName);
    }

    public static void RotateTowardsPlayer()
    {
        Vector3 direction = PlayerData.Position - _goInstance.transform.position;
        direction.Scale(new Vector3(1, 0, 1));
        _goInstance.transform.rotation = Quaternion.LookRotation(direction);
    }
    public static void RotateTowardsObject(Transform transform)
    {
        Vector3 direction = transform.position - _goInstance.transform.position;
        direction.Scale(new Vector3(1, 0, 1));
        _goInstance.transform.rotation = Quaternion.LookRotation(direction);
    }

    public static void FocusPlayerOnCompanion(bool b)
    {
        if (b)
            PlayerManager.SetDialogueRestrictions(instance._dialogueComponents.FocusObject, 5f);
        else
            PlayerManager.SetDialogueRestrictions(null);
    }
    public static void SetAnimationBool(string boolName, bool enabled)
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.SetAnimatorParam.SetBool(boolName, enabled);
    }

    public static void Kiss()
    {
        //play kiss animation
        if (_goInstance == null) return;
        if (_goInstance.TryGetComponent(out Companion companion))
        {
            companion.Kiss();
        }
    }

    private void OnSceneAwake()
    {

        bool alreadyHere = CompanionData.CurrentSceneLocation == SceneManager.GetActiveScene().name;

        var pos = CompanionData.LastKnownSpawnPosition;
        var rot = Quaternion.identity;


#if UNITY_EDITOR
        if (loadObjectPosition && GameObjectUtility.TryGetObjectTransform("CompanionDebugSpawn", out Transform transform))
        {
            pos = transform.position;
            rot = transform.rotation;
            Destroy(transform.gameObject);
        }
        else
#endif
            if (!alreadyHere)
            {
                GameObjectUtility.PurgeObjectsByTag(Prefab.tag);
                loadObjectPosition = false;
                return;
            }

        Spawn(pos, rot);
        SetInteractObject(DefaultInteractObject);


        loadObjectPosition = false;
    }
    #region spawning
    public static event Action OnCompanionSpawn;
    public static GameObject Spawn(Vector3 pos, Quaternion rot)
    {
        _goInstance = GameObjectUtility.SpawnUnique(pos, rot, Prefab);
        _companionBehaviorManager = _goInstance.GetComponent<CompanionBehaviorManager>();
        _interactSphere = _goInstance.GetComponent<Companion>()?.InteractSphere;
        CompanionAbilityManager.GameObjectInstance = _goInstance;
        CompanionAbilityManager.RefreshComponents();

        if (_goInstance.TryGetComponent(out Companion companion))
        {
            TabletCameraTarget = companion.TabletCamTarget;
        }

        OnCompanionSpawn?.Invoke();
        ResetData();
        return _goInstance;
    }
    public static void SetInteractIcon(Sprite sprite)
    {
        _interactSphere.Icon = sprite;
    }
    public static void RemoveCompanion()
    {
        if (_goInstance == null) return;
        DropCarriedItem();
        Destroy(_goInstance);
        _goInstance = null;
        GameObjectUtility.PurgeObjectsByTag(Prefab.tag);
        CompanionData.SceneGoingTo = null;
        CompanionData.CurrentSceneLocation = null;
    }
    #endregion

    #region setup_objects
    [SerializeField][Dependency] private ExecuteUnityEvent _defaultInteractObject;
    public static ExecuteUnityEvent DefaultInteractObject => instance._defaultInteractObject;
    private static ExecuteUnityEvent _interactObject = null;
    public static void ExecuteInteract()
    {
        if (_interactObject == null) return;
        _interactObject.Execute();
    }
    public static void SetInteractSphereTag(string tag)
    {
        _interactSphere.tag = tag;
    }
    public static ExecuteUnityEvent SetInteractObject(ExecuteUnityEvent interactObjectTemplate)
    {
        if (_interactObject != null) { Destroy(_interactObject.gameObject); }
        if (interactObjectTemplate == null) return null;

        _interactObject = GameObjectUtility.InstantiateAsChild(interactObjectTemplate.gameObject, _goInstance).GetComponent<ExecuteUnityEvent>();
        return _interactObject;
    }

    #endregion

    #region data management
    private static void ResetData()
    {
        CompanionData.SceneGoingTo = null;
        CompanionData.CurrentSceneLocation = SceneManager.GetActiveScene().name;
    }
    #endregion


    #region attacks and behavior

    #endregion
    [Command("spawn-companion")]
    private static void SpawnAtPlayer()
    {
        RemoveCompanion();
        Spawn(PlayerData.Position, PlayerData.Rotation);
        SetInteractObject(DefaultInteractObject);
    }

    public static void RaiseArms()
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.CompanionRigController.SetCarry(true);
    }
    public static void LowerArms()
    {
        if (_companionBehaviorManager != null)
            _companionBehaviorManager.CompanionRigController.SetCarry(false);
    }
}