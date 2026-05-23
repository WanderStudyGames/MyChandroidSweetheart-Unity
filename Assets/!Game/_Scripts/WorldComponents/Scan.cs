using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Scan : MonoBehaviour, IScannerSelectable
{
    [Dependency][SerializeField] private ScanMaterials scanMaterialsSO;
    [field: SerializeField] public string Name { get; private set; } = "";
    [field: FormerlySerializedAs("SecondaryColor")]
    [field: SerializeField] public Color Color { get; private set; } = Color.white;

    [Separator]

    [TextData("ScanTexts/", "Scan_")][SerializeField] private TextAsset textAsset;

    public Sprite Icon => ScanMaterials.Instance.scanSprite;

    private bool important;

    [SerializeField] private UnityEvent scanCompletedAction;
    [SerializeField] private bool performOnce;

    [Tooltip("Select additional game objects for which to create scan holograms.")]
    [SerializeField, Header("Holograms")] private List<GameObject> gameObjects = new();
    //private readonly List<GameObject> scanObjects = new();
    [Tooltip("Create scan holograms for all children of game objects.")]
    [SerializeField] private bool includeAllChildren;
    [SerializeField] private bool includeSelf = true;

    private Hologram _hologram;
    public Hologram Hologram => _hologram;

    private bool performed;


    public bool Completed { get; private set; }

    public bool Enabled => enabled;

    public void SetCompleted(bool b) { Completed = b; _hologram.material = scanMaterialsSO.scannedMaterial_HL; }

    private Material scanMaterial;


    public static event Action OnScanCompleted;
    public void ExecuteLeaveScanAction()
    {
        if (performOnce && performed) return;
        if (!Completed)
        {
            int currency = important ? 4000 : 1000;
            Wallets.Jolts.Add(currency);
            Completed = true;
            WorldData.CompletedScans.Add(textAsset.name);
            _hologram.material = scanMaterialsSO.scannedMaterial;
            OnScanCompleted?.Invoke();
        }
        Debug.LogWarning("SCAN COMLPETED ACTION");
        scanCompletedAction.Invoke();
        performed = true;
    }
    public string GetText()
    {
        return textAsset.text.Replace("<important>", "");
    }
    void Refresh()
    {
        if (textAsset == null) return;
        important = textAsset.text.Contains("<important>");
        scanMaterial = scanMaterialsSO.unscannedMaterial;
        Completed = WorldData.CompletedScans.Has(textAsset.name);
        if (Completed) performed = true;
        if (important) { scanMaterial = scanMaterialsSO.importantMaterial; }
        if (Completed) { scanMaterial = scanMaterialsSO.scannedMaterial; }
        if (_hologram != null) _hologram.material = scanMaterial;
    }
    private void Awake()
    {
        Refresh();

        if (!gameObjects.Contains(gameObject) && gameObject.activeInHierarchy && includeSelf)
        {
            gameObjects.Add(gameObject);
        }

        if (includeAllChildren)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (!gameObjects[i].activeInHierarchy) continue;

                foreach (Transform child in gameObjects[i].GetComponentsInChildren<Transform>())
                {
                    if (!child.gameObject.activeInHierarchy || gameObjects.Contains(child.gameObject)) continue;

                    gameObjects.Add(child.gameObject);
                }
            }
        }
        if (gameObject.GetComponent<Collider>() == null)
        {
            Collider col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
        }

        _hologram = gameObject.AddComponent<Hologram>();
        _hologram.material = scanMaterial;
        _hologram.objects = gameObjects.ToArray();
        _hologram.enabled = false;
    }

    private void OnScannerEnable()
    {
        if (PlayerStateManager.PreviousState == PlayerStates.Scanning) { Refresh(); }
        else { _hologram.enabled = true; }
    }
    private void OnEnable()
    {
        PlayerStates.Scanner.OnStateEnableEvent += OnScannerEnable;
        OnScanCompleted += Refresh;
        PlayerStateManager.OnStateChanged += OnStateChange;
        _hologram.enabled =
            PlayerStateManager.State == PlayerStates.Scanner ||
            PlayerStateManager.State == PlayerStates.Scanning ||
            PlayerStateManager.State == PlayerStates.Rewiring;
    }
    private void OnDisable()
    {
        OnScanCompleted -= Refresh;
        PlayerStates.Scanner.OnStateEnableEvent -= OnScannerEnable;
        PlayerStateManager.OnStateChanged -= OnStateChange;
        if (_hologram == null) return;
        _hologram.enabled = false;

    }
    private void OnStateChange(PlayerState state)
    {
        if (state != PlayerStates.Scanner && state != PlayerStates.Scanning && state != PlayerStates.Menu)
        {
            _hologram.enabled = false;
        }
    }
    private void OnDestroy()
    {

        scanCompletedAction.RemoveAllListeners();
    }

    public void SetHighlight(bool b)
    {
        Material norm = important ? scanMaterialsSO.importantMaterial : scanMaterialsSO.unscannedMaterial;
        Material hl = important ? scanMaterialsSO.importantMaterial_HL : scanMaterialsSO.unscannedMaterial_HL;
        if (Completed) { norm = scanMaterialsSO.scannedMaterial; hl = scanMaterialsSO.scannedMaterial_HL; }
        Material mat = b ? hl : norm;
        _hologram.material = mat;
    }
    public bool Select()
    {
        if (!enabled || PlayerStateManager.State != PlayerStates.Scanner) return false;
        SetHighlight(true);
        return true;
    }

    public void Deselect()
    {
        SetHighlight(false);
    }
    public void Click()
    {
        if (!enabled) return;
        ScanUI.Scan(this);
    }
    public void UnClick()
    {
        if (PlayerStateManager.State != PlayerStates.Scanning)
            ScanUI.LeaveScan();
    }
}
