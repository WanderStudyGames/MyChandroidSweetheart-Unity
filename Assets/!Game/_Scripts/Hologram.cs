using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hologram : MonoBehaviour
{
    //[SerializeField] private GameObject[] _objects;
    [SerializeField] private GameObject[] _objects = { };
    public GameObject[] objects { get { return _objects; } set { _objects = value; CreateHolograms(); } }
    [SerializeField] private Material _material;
    public Material material { get { return _material; } set { _material = value; SetMaterial(value); } }

    private List<GameObject> _holoObjects = new();
    //public void SetHoloObjects(GameObject[] objects) { _objects = objects; CreateHolograms(); }
    public void FadeOut(float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(Co_FadeOut());
        IEnumerator Co_FadeOut()
        {
            Color color = _material.color;
            yield return ExtensionMethods.Co_FadeFloat(seconds, new(_material.color.a, 0), fl =>
            {
                _material.color = new(
                    _material.color.r,
                    _material.color.g,
                    _material.color.b, fl
                    );
            });
            _material.color = color;
            enabled = false;
        }
    }
    private void Awake()
    {
        CreateHolograms();
    }//
    private void CreateHolograms()
    {
        foreach (var obj in _holoObjects)
        {
            Destroy(obj);
        }
        _holoObjects.Clear();

        var objs = _objects.ToList();
        for (int i = objs.Count - 1; i >= 0; i--)
        {
            if (objs[i] == null) objs.RemoveAt(i);
        }
        _objects = objs.ToArray();

        foreach (var obj in _objects)
        {
            if (TryCreateHologramDuplicate(obj, _material, out GameObject go))
            {
                _holoObjects.Add(go);
                go.SetActive(enabled);
            }
        }
    }
    public static bool TryCreateHologramDuplicate(GameObject gobj, Material hologramMaterial, out GameObject hologramGameObject)
    {
        hologramGameObject = null;
        MeshFilter foundMeshFilter = gobj.GetComponent<MeshFilter>();
        MeshRenderer foundMeshRenderer = gobj.GetComponent<MeshRenderer>();

        SkinnedMeshRenderer foundSkinnedRenderer = gobj.GetComponent<SkinnedMeshRenderer>();

        if (foundSkinnedRenderer == null)
        {
            if (foundMeshFilter == null || foundMeshRenderer == null) return false;
        }

        hologramGameObject = new(gobj.name + " (Hologram)");
        hologramGameObject.transform.SetParent(gobj.transform);
        hologramGameObject.transform.ResetLocal();

        if (foundSkinnedRenderer == null)
        {
            MeshFilter newMeshFilter = hologramGameObject.AddComponent<MeshFilter>();
            newMeshFilter.sharedMesh = foundMeshFilter.sharedMesh;
            MeshRenderer newMeshRenderer = hologramGameObject.AddComponent<MeshRenderer>();

            List<Material> mats = new();
            for (int i = 0; i < foundMeshRenderer.sharedMaterials.Length; i++) { mats.Add(hologramMaterial); }
            newMeshRenderer.sharedMaterials = mats.ToArray();
        }
        else if (foundSkinnedRenderer.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.On)
        {
            SkinnedMeshRenderer newSkinnedRenderer = hologramGameObject.AddComponent<SkinnedMeshRenderer>();

            newSkinnedRenderer.sharedMesh = foundSkinnedRenderer.sharedMesh;
            newSkinnedRenderer.rootBone = foundSkinnedRenderer.rootBone;
            newSkinnedRenderer.localBounds = foundSkinnedRenderer.localBounds;
            newSkinnedRenderer.bones = foundSkinnedRenderer.bones;
            newSkinnedRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;


            List<Material> mats = new();
            for (int i = 0; i < foundSkinnedRenderer.sharedMaterials.Length; i++) { mats.Add(hologramMaterial); }
            newSkinnedRenderer.sharedMaterials = mats.ToArray();

            newSkinnedRenderer.updateWhenOffscreen = foundSkinnedRenderer.updateWhenOffscreen;
            newSkinnedRenderer.allowOcclusionWhenDynamic = foundSkinnedRenderer.allowOcclusionWhenDynamic;
        }
        return hologramGameObject;
    }
    private void OnDestroy()
    {
        foreach (var obj in _holoObjects)
        {
            Destroy(obj);
        }
    }
    private void SetMaterial(Material material)
    {
        foreach (GameObject go in _holoObjects)
        {
            go.SetMaterials(material);
        }
    }
    private void OnEnable()
    {
        _holoObjects.SetActive(true);
    }
    private void OnDisable()
    {
        try
        {
            if (_holoObjects != null)
                _holoObjects.SetActive(false);
        }
        catch
        {
            Debug.LogError(gameObject.name, gameObject);
        }
    }
}