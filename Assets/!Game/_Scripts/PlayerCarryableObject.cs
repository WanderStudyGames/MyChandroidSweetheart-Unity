using UnityEngine;
using UnityEngine.Events;

public class PlayerCarryableObject : MonoBehaviour, IAttachableCharacter
{
    [Header("Hand Thumbnail Offset")]
    [SerializeField] private GameObject _thumbnailTemplate;
    public GameObject ThumbnailTemplate => _thumbnailTemplate;
    [SerializeField] private SFX _pickupSound;
    [SerializeField] private UnityEvent _onCarry;
    [SerializeField] private UnityEvent _onDrop;
    [field: SerializeField, Range(0, 1)] public float PlayerSpeedMultiplier { get; private set; } = 0.8f;
    [field: SerializeField] public bool OnlyPlaceOnNavMesh { get; private set; } = false;
    [SerializeField] private Mesh _mesh;
    private GameObject _thumbnailInstance;
    public Mesh Mesh => _mesh;
    private void Awake()
    {
    }
    public void Carry()
    {
        //if player isn't already holding something
        if (PlayerData.CarriedObject != null) return;


        //store component on player's SO data somewhere
        PlayerData.CarriedObject = this;

        if (_pickupSound != null) SFX.PlayAtPoint(_pickupSound, transform.position);

        //switch player to lifting state, store current state


        _onCarry.Invoke();
        //disable gameobject
        this.PhysicsFriendlyDisable(onDisable: () =>
        {
            PlayerStateManager.SwitchState(PlayerStates.Carrying);
        });
    }
    public void Drop(Vector3 position)
    {
        //if placement is valid (handled in state itself)
        //switch player to stored state
        PlayerStateManager.SwitchState(PlayerStates.Default);

        //re-enable gameobject, change position to placement position
        transform.position = position;
        gameObject.SetActive(true);
        Physics.SyncTransforms();


        //remove component from player's data
        PlayerData.CarriedObject = null;

        //remove hand thumbnail
        if (_thumbnailInstance != null) Destroy(_thumbnailInstance);

        _onDrop.Invoke();
        Detach();
    }

    public void AttachTo(GameObject go)
    {
        transform.SetParent(go.transform, true);
    }

    public void Detach()
    {
        transform.SetParent(null, true);
    }

    public void TeleportToClosest(Transform[] transforms)
    {
        var t = transforms.GetClosest(transform.position);
        transform.SetPositionAndRotation(t.position, t.rotation);
    }
}
