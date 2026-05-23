using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/Carrying", fileName = "Carrying Player State")]
public class CarryingPlayerState : PlayerState, ILookSelectorListener
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Material _material;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Sprite _placeActionSprite;
    //private GameObject _thumbnailInstance;
    private LerpToTransform _thumbnailLTT;
    [Dependency][SerializeField] private InteractProfile _interactProfile;
    public static InputActionMap[] Actions;
    private GameObject _hologram;
    private bool _success = false;
    private Vector3 _placePoint;
    private int _navMeshAreaMask;
    protected override void OnStateEnable()
    {
        _navMeshAreaMask = NavMesh.GetAreaFromName("Walkable");
        if (PlayerStateManager.PreviousState != PlayerStates.Default && PlayerStateManager.PreviousState != PlayerStates.Scanner)
            _context.PlayerInput.actions.SetMaps(InputMaps.Default);
        PlayerLookSelector.PrimaryInputListenerList.AddListener(this);
        Debug.Assert(PlayerData.CarriedObject != null, "No carried object in CarryingPlayerState!");
        PlayerMove.SpeedMultis.Add(new(PlayerData.CarriedObject.PlayerSpeedMultiplier, "Carry"));

        _context.PlayerLookSelector.OnTriggerHit += OnRaycastTrigger;

        NavMeshVisualize.EnableVisuals(true);

        _hologram = new GameObject();
        _hologram.AddComponent<MeshFilter>().sharedMesh = PlayerData.CarriedObject.Mesh;
        var r = _hologram.AddComponent<MeshRenderer>();
        var list = new List<Material>();
        for (int i = 0; i < PlayerData.CarriedObject.Mesh.subMeshCount; i++)
        {
            list.Add(_material);
        }
        r.sharedMaterials = list.ToArray();
        if (_thumbnailLTT != null) Destroy(_thumbnailLTT.gameObject);
        //set hand thumbnail

        _thumbnailLTT = Instantiate(PlayerData.CarriedObject.ThumbnailTemplate).GetOrAddComponent<LerpToTransform>();
        _thumbnailLTT.transform.position = PlayerData.CarriedObject.transform.position;
        _thumbnailLTT.StartCoroutine(Co_SpawnThumbnail());
        IEnumerator Co_SpawnThumbnail()
        {
            yield return null;
            yield return null;
            _thumbnailLTT.SetTarget(PlayerData.UIHandUp, 20f);
        }

        _success = false;

    }
    public bool ValidateInputAction() => _success;
    public void OnInputAction(InputAction.CallbackContext ctx)
    {
        if (!ctx.action.WasPressedThisFrame()) { return; }
        if (_success)
        {
            Debug.Log(PlayerData.CarriedObject);
            PlayerData.CarriedObject.Drop(_placePoint);
        }
    }
    protected override void OnStateDisable(PlayerState destinationState)
    {
        PlayerMove.SpeedMultis.RemoveByName("Carry");
        Destroy(_hologram);
        _success = false;
        OnStopHover?.Invoke();
        PlayerLookSelector.PrimaryInputListenerList.RemoveListener(this);
        _context.PlayerLookSelector.OnTriggerHit -= OnRaycastTrigger;

        Debug.Log("Disabling Carrying State");
        Destroy(_thumbnailLTT.gameObject);
        _thumbnailLTT = null;

        NavMeshVisualize.EnableVisuals(false);
    }
    private void OnRaycastTrigger(RaycastHit hit)
    {
        bool inLayerMask = hit.collider != null && _layerMask.Contains(hit.collider.gameObject.layer);
        if (hit.distance <= 0 || hit.distance > 4f || !inLayerMask) { _hologram.transform.position = new(-100, -100, -100); _success = false; OnStopHover?.Invoke(); return; }
        //if highlighting pad, success
        else if (hit.collider.isTrigger && hit.collider.gameObject.name == "raycast_dogCompatible")
        {
            _placePoint = hit.collider.gameObject.transform.position;
            _hologram.transform.position = _placePoint;
            _material.color = _successColor;
            _success = true;
            OnHover?.Invoke(_placeActionSprite);
        }
        //else if hitting physical surface, check for upright
        else if (!hit.collider.isTrigger)
        {
            _placePoint = hit.point;
            bool b = true;
            if (PlayerData.CarriedObject.OnlyPlaceOnNavMesh)
            {
                if (!NavMesh.SamplePosition(_placePoint, out _, 0.2f, 1 << 0)) b = false;
            }
            _hologram.transform.position = _placePoint;

            if (Vector3.Angle(hit.normal, Vector3.up) < 30f && b)
            {
                _material.color = _successColor;
                _success = true;
                OnHover?.Invoke(_placeActionSprite);
            }
            else { _material.color = _failColor; _success = false; OnStopHover?.Invoke(); }
        }




    }
    public event Action<Sprite> OnHover;
    public event Action OnStopHover;
}

