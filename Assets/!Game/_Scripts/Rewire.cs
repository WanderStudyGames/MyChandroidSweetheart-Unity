using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Rewire : MonoBehaviour, IScannerSelectable
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private SignalOutput _signalOutput;
    [SerializeField] private Transform _lineOrigin;

    private Hologram _hologram;
    private Hologram _inputHologram;

    public Sprite Icon => ScanMaterials.Instance.rewireSprite;
    public bool Enabled => enabled;

    private LineRenderer _lineRenderer;
    private void Awake()
    {
        _hologram = gameObject.AddComponent<Hologram>();
        _hologram.objects = _objects;
        _hologram.material = ScanMaterials.Instance.rewireMaterial;
        _hologram.enabled = false;
        if (_lineOrigin == null) _lineOrigin = transform;

        if (_signalOutput.SignalInputs.Count > 0)
        {
            var input = _signalOutput.SignalInputs[0];
            if (_inputHologram != null)
            {
                Destroy(_inputHologram);
            }
            _inputHologram = input.gameObject.GetOrAddComponent<Hologram>();
            _inputHologram.material = ScanMaterials.Instance.rewireTarget_Inactive;
            _inputHologram.objects = input.RewireTarget.HologramObjects;
            _inputHologram.enabled = false;
        }



        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.sharedMaterial = ScanMaterials.Instance.rewireCable;
        _lineRenderer.widthMultiplier = 0.15f;
        _lineRenderer.numCapVertices = 3;
        _lineRenderer.enabled = false;
        _lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        UpdateLineRenderer();
        foreach (var inp in _signalOutput.SignalInputs)
        {
            inp.UpdateLineRenderer += UpdateLineRenderer;
        }
        //StartCoroutine(Co_UpdateLineRenderer());
    }

    public void UpdateLineRenderer()
    {
        if (_signalOutput.SignalInputs.Count == 0) return;
        Vector3[] positions = new Vector3[] { _lineOrigin.position, _signalOutput.SignalInputs[0].RewireTarget.HologramRayTarget.position };
        _lineRenderer.SetPositions(positions);
    }
    public void Link(SignalInput input)
    {
        if (_signalOutput.onner)
        {
            foreach (var inp in _signalOutput.SignalInputs)
            {
                inp.Off();
            }
            input.On();
        }

        if (_inputHologram != null)
        {
            Destroy(_inputHologram);
        }
        _inputHologram = input.gameObject.AddComponent<Hologram>();
        _inputHologram.material = ScanMaterials.Instance.rewireTarget_Inactive;
        _inputHologram.objects = input.RewireTarget.HologramObjects;

        foreach (var inp in _signalOutput.SignalInputs)
        {
            inp.UpdateLineRenderer -= UpdateLineRenderer;
        }
        _signalOutput.SignalInputs.Clear();
        _signalOutput.SignalInputs.Add(input);
        input.UpdateLineRenderer += UpdateLineRenderer;

        StopAllCoroutines();
        UpdateLineRenderer();
        //StartCoroutine(Co_UpdateLineRenderer());
        EnableHolos();

        SFX.PlayAtPoint(PlayerStates.Rewiring.EndSFX, Vector3.zero);
    }
    private void OnEnable()
    {
        PlayerStates.Scanner.OnStateEnableEvent += EnableHolos;
        PlayerStateManager.OnStateChanged += OnStateChange;
        UpdateLineRenderer();
    }
    private void OnDisable()
    {
        PlayerStates.Scanner.OnStateEnableEvent -= EnableHolos;
        PlayerStateManager.OnStateChanged -= OnStateChange;
    }
    private void OnStateChange(PlayerState state)
    {
        if (state != PlayerStates.Scanner && state != PlayerStates.Menu && state != PlayerStates.Scanning && state != PlayerStates.Rewiring)
        {
            DisableHolos();
        }
    }
    private void EnableHolos()
    {
        if (!Inventories.Instance.PlayerInventory.Has("Rewire Module")) { return; }
        _hologram.enabled = true;
        if (_inputHologram != null) _inputHologram.enabled = true;
        if (_signalOutput.SignalInputs.Count == 1) _lineRenderer.enabled = true;
        _hologram.material = ScanMaterials.Instance.rewireMaterial;
        _lineRenderer.material = ScanMaterials.Instance.rewireCable;

    }
    private void DisableHolos()
    {
        _hologram.enabled = false;
        if (_inputHologram != null) _inputHologram.enabled = false;
        _lineRenderer.enabled = false;
    }

    private void RewiringModeHolos()
    {
        EnableHolos();
        _hologram.material = ScanMaterials.Instance.rewireMaterial_Clicked;
        if (_inputHologram != null) _inputHologram.enabled = false;
        _lineRenderer.material = ScanMaterials.Instance.rewireMaterial_Clicked;
    }
    private void NormalHolos()
    {
        _hologram.material = ScanMaterials.Instance.rewireMaterial;
        if (_inputHologram != null)
        {
            _inputHologram.material = ScanMaterials.Instance.rewireTarget_Inactive;
            _inputHologram.enabled = true;
        }
        _lineRenderer.material = ScanMaterials.Instance.rewireCable;
    }

    public bool Select()
    {
        if (!enabled) return false;
        if (!_hologram.enabled) return false;
        if (PlayerStateManager.State != PlayerStates.Scanner) return false;
        if (!Inventories.Instance.PlayerInventory.Has("Rewire Module")) return false;
        _hologram.material = ScanMaterials.Instance.rewireMaterial_HL;
        return true;
    }

    public void Deselect()
    {
        if (PlayerStateManager.State != PlayerStates.Scanner || !Inventories.Instance.PlayerInventory.Has("Rewire Module")) return;
        NormalHolos();
    }

    public void Click()
    {
        if (!enabled) return;
        if (!_hologram.enabled) return;
        if (PlayerStateManager.State != PlayerStates.Scanner) return;
        if (!Inventories.Instance.PlayerInventory.Has("Rewire Module")) return;
        RewiringPlayerState.Rewire = this;
        PlayerStateManager.SwitchState(PlayerStates.Rewiring);
        RewiringModeHolos();
    }

    public void UnClick()
    {
    }
}
