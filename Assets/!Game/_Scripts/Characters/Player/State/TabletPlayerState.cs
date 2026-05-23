using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Tablet", fileName = "Tablet Player State")]
public class TabletPlayerState : PlayerState
{
    public static InputActionMap[] Actions;
    private Transform _oldParent;
    private Transform _oldRotationTransform;
    private GameObject _anchor;
    private bool hasRangeUpgrade;
    public static Sprite UI;
    public static bool CompanionHeld;
    [SerializeField] private float MaxDistanceFromCompanion = 15f;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventoryItemMetadata _rangeIncreaseItem;
    public static bool Jammed = false;
    [ContextMenu("Enable")]
    protected override void OnStateEnable()
    {
        hasRangeUpgrade = _inventory.Has(_rangeIncreaseItem);
        _context.PlayerInput.actions.SetMaps(InputMaps.Default);
        var act = _context.PlayerInput.actions;
        act["Interact"].Disable();
        act["Tool"].Disable();
        act["Glasses"].Disable();
        act["Laser"].Disable();
        act["Move"].Disable();
        act["Jump"].Disable();

        CompanionStealer.OnCompanionStolen += Leave;

        if (CompanionHeld)
        {
            CompanionManager.RaiseArms();
        }



        if (PlayerStateManager.PreviousState != PlayerStates.Tablet)
        {
            _anchor = new GameObject("TabletAnchor");

            _oldParent = PlayerLook.Camera.transform.parent;
            PlayerLook.Camera.transform.SetParent(_anchor.transform, false);

            _anchor.transform.position = CompanionManager.TabletCameraTarget.position;
            var ltt = _anchor.GetOrAddComponent<LerpToTransform>();
            ltt.IncludeRotation = false;
            ltt.SetTarget(CompanionManager.TabletCameraTarget, CompanionHeld ? 1000f : 2f);
            _oldRotationTransform = PlayerLook.GetRotationTransform();
            PlayerLook.SetRotationTransform(_anchor.transform);
            var euler = _anchor.transform.rotation.eulerAngles;
            euler.y = CompanionManager.TabletCameraTarget.transform.rotation.eulerAngles.y;
            _anchor.transform.SetPositionAndRotation(PlayerLook.Camera.transform.position, Quaternion.Euler(euler));
        }
        if (!CompanionHeld)
        {
            _context.PlayerInput.actions.Link("Tablet", OnTabletLeave);
            _context.PlayerInput.actions.Link("Follow", OnPlaceObject);
        }
    }
    protected override void OnStateDisable(PlayerState destinationState)
    {
        if (destinationState != PlayerStates.Tablet && destinationState != null)
        {
            Debug.Log($"PLAYER STATE: {destinationState}");
            PlayerLook.SetRotationTransform(_oldRotationTransform);
            PlayerLook.Camera.transform.SetParent(_oldParent, false);
            Destroy(_anchor);
            _anchor = null;
            var act = _context.PlayerInput.actions;
            act["Interact"].Enable();
            act["Glasses"].Enable();
            act["Laser"].Enable();
            act["Tool"].Enable();
            act["Move"].Enable();
            act["Jump"].Enable();
            CompanionStealer.OnCompanionStolen -= Leave;

            if (CompanionHeld)
            {
                CompanionManager.LowerArms();

                PlayerManager.Teleport(CompanionManager.TabletCameraTarget.parent.position + Vector3.up, CompanionManager.TabletCameraTarget.parent.rotation);
                CompanionManager.FollowPlayer();
            }
        }
        if (!CompanionHeld)
        {
            _context.PlayerInput.actions.UnLink("Tablet", OnTabletLeave);
            _context.PlayerInput.actions.UnLink("Follow", OnPlaceObject);
        }
    }
    public override void OnUpdate()
    {
        if (!CompanionManager.CompanionEnabled || Jammed)
        {
            Leave();
            return;
        }
        if (CompanionHeld || hasRangeUpgrade) return;
        if (Vector3.Distance(CompanionData.Position, PlayerData.Position) > MaxDistanceFromCompanion)
        {
            Leave();
        }
    }
    private void OnTabletLeave(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Leave();
        }
    }
    private void OnPlaceObject(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            CompanionManager.DropCarriedItem();
        }
    }
    private void Leave()
    {
        PlayerStateManager.SwitchState(PlayerStates.Default);
    }
}

