using UnityEngine;
[RequireComponent(typeof(Collider))]
public class RewireTarget : MonoBehaviour, IScannerSelectable
{
    [Dependency][SerializeField] private SignalInput _signalInput;

    private Hologram _hologram;

    [field: SerializeField] public GameObject[] HologramObjects { get; private set; }

    [field: SerializeField] public Transform HologramRayTarget { get; private set; }
    public Sprite Icon => ScanMaterials.Instance.rewireTargetSprite;
    public bool Enabled => enabled;


    private void Awake()
    {
        _hologram = gameObject.AddComponent<Hologram>();
        _hologram.material = ScanMaterials.Instance.rewireTarget;
        _hologram.objects = HologramObjects;
        _hologram.enabled = false;
        if (HologramRayTarget == null) HologramRayTarget = transform;
    }
    private void OnEnable()
    {
        PlayerStates.Rewiring.OnStateEnableEvent += EnableHolos;
        PlayerStates.Rewiring.OnStateDisableEvent += DisableHolos;
    }
    private void OnDisable()
    {
        PlayerStates.Rewiring.OnStateEnableEvent -= EnableHolos;
        PlayerStates.Rewiring.OnStateDisableEvent -= DisableHolos;
    }
    private void EnableHolos() { _hologram.enabled = true; }
    private void DisableHolos() { _hologram.enabled = false; }

    public bool Select()
    {
        if (PlayerStateManager.State != PlayerStates.Rewiring) return false;
        _hologram.material = ScanMaterials.Instance.rewireTarget_HL;
        return true;
    }

    public void Deselect()
    {
        _hologram.material = ScanMaterials.Instance.rewireTarget;
    }

    public void Click()
    {
        if (PlayerStateManager.State != PlayerStates.Rewiring) return;
        RewiringPlayerState.Rewire.Link(_signalInput);
        PlayerStateManager.SwitchState(PlayerStates.Scanner);
    }

    public void UnClick()
    {
    }
    private void OnDrawGizmosSelected()
    {
        if (HologramRayTarget == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(HologramRayTarget.position, Vector3.one * 0.5f);
    }
}
