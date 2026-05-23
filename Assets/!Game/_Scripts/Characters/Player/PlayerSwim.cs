using UnityEngine;

[RequireComponent(typeof(CharacterControllerMove))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(Player))]
public class PlayerSwim : MonoBehaviour
{
    private bool _isSwimming;
    public bool IsSwimming => _isSwimming;
    private CharacterControllerMove _characterControllerMove;
    [SerializeField] private PlayerMovementProfile _playerMovementProfile;

    [SerializeField] private PlayerSwimProfile playerSwimProfile;
    private PlayerAudio _playerAudio;
    private bool _drowningEnabled;
    private bool _drowned;
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private Player _player;

    private void Awake()
    {
        _characterControllerMove = GetComponent<CharacterControllerMove>();
        _playerAudio = GetComponent<PlayerAudio>();
        _drowningEnabled = !Inventories.Instance.PlayerInventory.Has(_playerMovementProfile.PreventDrownItem);
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        Inventory.OnInventoryChange += OnItemAdded;
    }
    private void OnItemAdded(Inventory inventory, InventoryItem item, bool has)
    {
        if (inventory != Inventories.Instance.PlayerInventory) return;
        if (item.Metadata != _playerMovementProfile.PreventDrownItem) return;
        _drowningEnabled = !has;
    }
    private void OnDisable()
    {
        Inventory.OnInventoryChange -= OnItemAdded;
    }

    public bool UpdateIsSwimming()
    {
        if (Physics.SphereCast(transform.position + Vector3.up, _characterControllerMove.Radius, Vector3.down, out RaycastHit hit, 5, _playerMovementProfile.groundLayers, QueryTriggerInteraction.Ignore))
        {
            _isSwimming = hit.collider.gameObject.layer == LayerMask.NameToLayer("Water");
            if (_isSwimming && _drowningEnabled && _characterControllerMove.IsGrounded && !_drowned) { Drown(); }
            return _isSwimming;
        }
        return false;
    }
    private void Drown()
    {
        _drowned = true;
        _player.Die(playerSwimProfile.DrowningSprite, _playerMovementProfile.drownSFX);
        //_playerMove.SetDisableControls(true);
        //_playerJump.SetDisableGravity(true);
        //GlobalPlayerInput.Instance.enabled = false;
        //DeathUI.SetSprite(playerSwimProfile.DrowningSprite);
        //UIFade.FadeColor = Color.black;
        //UIFade.FadeDurations = new(0, 0, 1);
        //MusicManager.KillBGM();
        //UIFade.ExecuteAfterFade(() =>
        //{
        //    StartCoroutine(Co_DeathWait());

        //});
        //IEnumerator Co_DeathWait()
        //{
        //    yield return new WaitForSecondsRealtime(4);
        //    SceneHandler.ResetScene();
        //    _characterControllerMove.SetVelocity(Vector3.zero);
        //    yield break;
        //}
        //DontDestroyOnLoad(_playerMovementProfile.drownSFX.PlayAtPoint(transform.position));
    }
    private void Update()
    {
        bool oldsw = _isSwimming;
        UpdateIsSwimming();
        if (_characterControllerMove.IsGrounded && oldsw != _isSwimming)
        {
            _playerMovementProfile.OnExitWaterGE.Raise();
            _playerAudio.Land("", 0);
        }
    }
}