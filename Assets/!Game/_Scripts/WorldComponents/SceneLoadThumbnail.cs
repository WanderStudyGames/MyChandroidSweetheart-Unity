using UnityEngine;

#if UNITY_EDITOR

using System.Linq;

using UnityEditor;

#endif

[RequireComponent(typeof(MeshRenderer))]
public class SceneLoadThumbnail : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    [SerializeField] private bool overrideParallax;
    [SerializeField] private float _parallax;
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
        ReplaceTexture();
    }
    private void OnValidate()
    {
        ReplaceTexture();
    }
    private void OnEnable()
    {
        ReplaceTexture();
    }
    [ContextMenu("ReplaceTexture()")]
    void ReplaceTexture()
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
            if (overrideParallax) MeshRenderer.material.SetFloat("_Parallax", _parallax);
            MeshRenderer.material.mainTexture = _texture;
        }
        else
        {
            if (MeshRenderer.sharedMaterial == null) return;
            MeshRenderer.sharedMaterial.mainTexture = _texture;
        }
    }
    private void OnDrawGizmosSelected()
    {
        ReplaceTexture();

    }
}
