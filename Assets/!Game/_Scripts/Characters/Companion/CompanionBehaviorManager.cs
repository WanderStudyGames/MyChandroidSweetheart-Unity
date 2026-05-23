using System;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CompanionBehaviorManager : MonoBehaviour
{
    [SerializeField] private InteractibleObject _interactSphere;
    public InteractibleObject InteractSphere => _interactSphere;
    [SerializeField] private SetAnimatorParam _setAnimatorParam;
    public SetAnimatorParam SetAnimatorParam => _setAnimatorParam;
    [SerializeField] private CompanionRigController _companionRigController;
    public CompanionRigController CompanionRigController => _companionRigController;
    [SerializeField] private PlayerCarryableObject _playerCarryableObject;
    [SerializeField] private InteractibleObject _interactibleObject;
    public PlayerCarryableObject PlayerCarryableObject => _playerCarryableObject;
    public static event Action<CompanionBehavior> OnBehaviorChanged;
    private CompanionBehavior _currentBehavior;
    public CompanionBehavior CurrentBehavior => _currentBehavior;
    private NavMeshAgent _navMeshAgent;
    private Companion _companion;
    public Companion Companion => _companion;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _companion = GetComponent<Companion>();
        SetBehavior(CompanionManager.CompanionBehaviors.FollowBehavior);
    }
    public void SetBehavior(CompanionBehavior behavior)
    {
        if (_currentBehavior == behavior) return;
        if (_currentBehavior != null) _currentBehavior.Disable();
        _currentBehavior = behavior;
        behavior.Context = this;
        _interactSphere.SetCanInteract(behavior.CanInteract());
        _navMeshAgent.SetValues(behavior.NMAProfile);
        behavior.Enable();
        OnBehaviorChanged?.Invoke(behavior);
    }
    public void OnInteract()
    {
        _currentBehavior.OnInteract();
    }
    public void IgnoreInteractEmbargos(bool ignore)
    {
        _interactibleObject.IgnoreEmbargos(ignore);
    }
    private void Update()
    {
        _currentBehavior.OnUpdate();
    }
    private void OnTriggerStay(Collider other)
    {
        if (_currentBehavior != null)
            _currentBehavior.OnTriggerStay(other);
    }
    public void Scare()
    {
        SetBehavior(CompanionManager.CompanionBehaviors.FearBehavior);
    }
    private void OnEnable()
    {
        if (Companion.CarriedObject == null)
        {
            CompanionRigController.SetCarry(false);
        }
        CompanionData.Command(transform.position);
        Companion.OnCarry += OnCarry;
        Companion.OnDrop += OnDrop;
    }
    private void OnDisable()
    {
        Companion.OnCarry -= OnCarry;
        Companion.OnDrop -= OnDrop;
    }
    private void OnCarry()
    {
        _interactSphere.SetCanInteract(true);
    }
    private void OnDrop()
    {
        _interactSphere.SetCanInteract(_currentBehavior.CanInteract());
    }
}