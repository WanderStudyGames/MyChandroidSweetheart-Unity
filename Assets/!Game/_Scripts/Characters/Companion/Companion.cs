using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CompanionBehaviorManager))]
public class Companion : Character, IAttachableCharacter
{
    [Dependency][SerializeField] private CompanionData _CompanionData;
    [Dependency][SerializeField] private SceneHandler sceneHandler;
    [Dependency][SerializeField] private GameObject enemyPF;
    [Dependency][SerializeField] private CompanionInteractDetector companionInteractDetector;
    [field: SerializeField] public Sprite DropSprite { get; private set; }
    [field: SerializeField] public Sprite DialogueSprite { get; private set; }
    [field: SerializeField] public Transform TabletCamTarget;
    [field: SerializeField] public Transform CarryingPosition;
    [SerializeField] private InteractibleObject _interactSphere;
    public InteractibleObject InteractSphere => _interactSphere;
    [SerializeField] private CompanionBehaviorManager _companionBehaviorManager;
    public CompanionInteractDetector CompanionInteractDetector => companionInteractDetector;
    [Dependency][SerializeField] private GameObject followInPF;
    [SerializeField] private SFX followSFX;
    [SerializeField] private UnityEvent OnKiss;
    private AudioSource audioSource;


    private static GameObject followIndicator;
    private NavMeshAgent NMAgent;

    private void OnEnable()
    {
        Commands.OnCommand += OnCommand;
        LaserPointer.OnCommand += OnCommand;
        PlayerStates.Scanning.OnStateEnableEvent += Freeze;
        PlayerStates.Scanning.OnStateDisableEvent += UnFreeze;
        SceneHandler.BeforeSceneLoad += BeforeSceneLoad;

        StartCoroutine(Co_OneFrame());

        IEnumerator Co_OneFrame()
        {
            yield return null;
            if (_companionBehaviorManager.CurrentBehavior == null) _companionBehaviorManager.SetBehavior(CompanionManager.CompanionBehaviors.FollowBehavior);
            else _companionBehaviorManager.SetBehavior(_companionBehaviorManager.CurrentBehavior);
        }

    }
    private void OnDisable()
    {
        Commands.OnCommand -= OnCommand;
        LaserPointer.OnCommand -= OnCommand;
        PlayerStates.Scanning.OnStateEnableEvent -= Freeze;
        PlayerStates.Scanning.OnStateDisableEvent -= UnFreeze;
        SceneHandler.BeforeSceneLoad -= BeforeSceneLoad;
    }
    public void UnFreeze() { NMAgent.enabled = true; }
    public void Freeze() { NMAgent.enabled = false; }

    void Awake()
    {
        NMAgent = GetComponent<NavMeshAgent>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        _companionBehaviorManager = GetComponent<CompanionBehaviorManager>();
        _interactSphere.IgnoreEmbargos(false);
    }

    public override void AttachTo(GameObject gameObject)
    {
        NMAgent.enabled = false;
        base.AttachTo(gameObject);
        OnAttach?.Invoke(gameObject);
        //play "waiting with hand on hip" animation
    }
    private CompanionCarryable _companionCarryable;
    public CompanionCarryable CarriedObject
    {
        get { return _companionCarryable; }
        set
        {
            _companionCarryable = value;
            bool b = value != null;
            _companionBehaviorManager.CompanionRigController.SetCarry(b);
            var pos = NMAgent.destination;
            NMAgent.enabled = false;
            transform.position = pos;
            NMAgent.enabled = true;
            //if (NMAgent.enabled) NMAgent.SetDestination(transform.position);
            if (b)
            {
                OnCarry?.Invoke();
                _interactSphere.IgnoreEmbargos(true);
                CompanionManager.SetInteractIcon(DropSprite);
            }
            else
            {
                OnDrop?.Invoke();
                _interactSphere.IgnoreEmbargos(false);
                CompanionManager.SetInteractIcon(DialogueSprite);
            }
        }
    }
    public event Action OnCarry;
    public event Action OnDrop;
    public void TeleportToClosest(Transform[] transforms)
    {
        var e = NMAgent.enabled;
        NMAgent.enabled = false;
        var t = transforms.GetClosest(transform.position);
        transform.SetPositionAndRotation(t.position, t.rotation);
        NMAgent.enabled = e;
        CompanionManager.StandInPlace();
    }
    public void DropCarriedItem()
    {
        NMAgent.SetDestination(transform.position);
        CompanionData.Command(null);
        _companionBehaviorManager.SetBehavior(CompanionManager.CompanionBehaviors.IdleBehavior);
        NMAgent.isStopped = true;

        if (CarriedObject == null) return;
        NMAgent.enabled = false;
        transform.position -= transform.forward;
        NMAgent.enabled = true;
        CarriedObject.Drop(transform.position + transform.forward);

    }
    public override void Detach()
    {
        StartCoroutine(NMAgent.Co_EnableWhenNavMeshExists());
        //CompanionData.Command();
        OnDetach?.Invoke();
        base.Detach();
    }
    void OnCommand()
    {
        companionInteractDetector.SetTarget(null);
        Command command = CompanionData.CurrentCommand;
        if (command.Transform != null) { _companionBehaviorManager.CurrentBehavior.OnCommand(command.Transform); return; }
        if (command.Vector3 != Vector3.zero) { _companionBehaviorManager.CurrentBehavior.OnCommand(command.Vector3); return; }
    }

    public List<ICompanionAttackable> AttackTargets { get; } = new();
    public void AddAttackTarget(ICompanionAttackable attackable) { AttackTargets.AddUnique(attackable); }
    public void RemoveAttackTarget(ICompanionAttackable attackable) { AttackTargets.RemoveAll(attackable); }

    public void Attack(ICompanionAttackable attackable)
    {
        AddAttackTarget(attackable);
        _companionBehaviorManager.SetBehavior(CompanionManager.CompanionBehaviors.AttackBehavior);
    }
    public void CheckSpawnBeforeSceneLoad(string sceneName)
    {
        //find sufficient reason why player would ever leave chandroid in a room
        //
        //if (NMAgent.enabled && Vector3.Distance(NMAgent.pathEndPosition, PlayerData.Position) < 1)
        //{
        //    CompanionData.SceneGoingTo = sceneName;
        //}
        CompanionData.SceneGoingTo = sceneName;
        CompanionData.RecordPosition(transform.position);
    }
    public void DoFollowIndicator()
    {
        StartCoroutine(Co_Indicator());
        IEnumerator Co_Indicator()
        {
            yield return new WaitForSeconds(0.14f);
            if (followIndicator == null)
            {
                audioSource.PlaySFX(followSFX);
                followIndicator = Instantiate(followInPF);
                followIndicator.transform.SetParent(transform, false);
                followIndicator.transform.ResetLocal();
            }
        }
    }
    public void BeforeSceneLoad(string sceneName)
    {
        CheckSpawnBeforeSceneLoad(sceneName);
    }
    private void Update()
    {
        CompanionData.Position = transform.position;

    }
    public void Kiss()
    {
        OnKiss.Invoke();
    }
}
