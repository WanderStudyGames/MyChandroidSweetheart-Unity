using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using VInspector;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class AttachMeshesToArmature : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _parent;
    [SerializeField] private Scan _scan;
    [SerializeField] private Hologram _hologram;
    [SerializeField] private Outfit _outfit;
    [SerializeField] private bool _spawnHairAndAccessoriesOnly;
    [SerializeField] private SkinnedMeshRenderer _target;

    private GameObject[] _defaultScanObjects = new GameObject[0];
    private GameObject[] _defaultHoloObjects = new GameObject[0];
    private void Awake()
    {

    }
    [Button]
    public void AttachTarget()
    {
        _target.ReassignArmature(_parent);
    }
    private SkinnedMeshRenderer[] _smRenderers;
    private void OnEnable()
    {
        if (_outfit != null && _outfit.Clothings != null)
            _outfit.Clothings.OnDataModified += OnOutfitChange;
        OnOutfitChange();
    }
    private void Start()
    {
        if (_scan != null && _scan.Hologram != null) _defaultScanObjects = _scan.Hologram.objects;
        if (_hologram != null && _hologram.objects != null) _defaultHoloObjects = _hologram.objects;
        AttachAll();
    }
    private void OnDisable()
    {
        if (_outfit != null)
            _outfit.Clothings.OnDataModified -= OnOutfitChange;
    }
    [Button]
    private void AttachAll()
    {
#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Attach Meshes to Armature");
#endif

        if (_smRenderers != null && _smRenderers.Length > 0)
        {
            foreach (var renderer in _smRenderers)
            {
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(renderer.gameObject);
#else
                Destroy(renderer.gameObject);
#endif
            }
        }

        if (_outfit != null)
            _smRenderers = _outfit.SpawnClothings(_parent, _spawnHairAndAccessoriesOnly);

        SetHolograms();

        var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var smRenderer in renderers)
        {
#if UNITY_EDITOR
            Undo.RecordObject(smRenderer, "Reassign Armature");
#endif
            smRenderer.ReassignArmature(_parent);
#if UNITY_EDITOR
            EditorUtility.SetDirty(smRenderer);
#endif
        }

#if UNITY_EDITOR
        Undo.IncrementCurrentGroup();
#endif
    }
    private void SetHolograms()
    {
        if (_scan != null && _scan.Hologram != null)
        {
            var list = new List<GameObject>();
            foreach (SkinnedMeshRenderer s in _smRenderers)
            {
                list.AddUnique(s.gameObject);
            }
            _scan.Hologram.objects = _defaultScanObjects.Concat(list).ToArray();
        }
        if (_hologram != null)
        {
            var list = new List<GameObject>();
            foreach (SkinnedMeshRenderer s in _smRenderers)
            {
                list.AddUnique(s.gameObject);
            }
            _hologram.objects = _defaultHoloObjects.Concat(list).ToArray();
        }
    }
    private void OnOutfitChange()
    {

        StartCoroutine(OneFrame());
        IEnumerator OneFrame()
        {
            yield return new WaitForEndOfFrame();
            //foreach (var smRenderer in _smRenderers) { Destroy(smRenderer.gameObject); }
            //_smRenderers = _outfit.SpawnClothings(_parent);
            AttachAll();
        }
    }
}
