#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PortalThumbnail : MonoBehaviour
{
    [SerializeField] private Cubemap _cubemap;
    private MeshRenderer _meshRenderer;

    private MeshRenderer MeshRenderer
    {
        get
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            return _meshRenderer;
        }
        set { _meshRenderer = value; }
    }
    private void Awake()
    {
#if UNITY_EDITOR
        if (Selection.gameObjects.Contains(gameObject))
        {
            Selection.objects = null;
        }
#endif
        ReplaceCubemap();
    }
    private void OnValidate()
    {
        ReplaceCubemap();
    }
    private void OnEnable()
    {
        ReplaceCubemap();
    }
    [ContextMenu("ReplaceCubemap()")]
    void ReplaceCubemap()
    {
        if (Application.isPlaying)
        {
            var b =
#if UNITY_EDITOR
             PrefabUtility.IsPartOfPrefabAsset(this) ||
#endif
                MeshRenderer.material == null;


            if (b)
            {
                return;
            }
            MeshRenderer.material.SetTexture("_Cubemap", _cubemap);
        }
        else
        {
            if (MeshRenderer.sharedMaterial == null) return;
            MeshRenderer.sharedMaterial.SetTexture("_Cubemap", _cubemap);
        }
    }
    private void OnDrawGizmosSelected()
    {
        ReplaceCubemap();

    }
}
