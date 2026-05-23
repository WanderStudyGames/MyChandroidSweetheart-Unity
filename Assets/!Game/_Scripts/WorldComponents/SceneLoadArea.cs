
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SceneLoader))]
public class SceneLoadArea : MonoBehaviour
{
    [SerializeField] private SFX _enterLoadAreaSFX;
    private SceneLoader sceneLoader;
    private void Awake()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag))
        {
            PlayerManager.SetMovementEnabled(false);
            PlayerManager.SetCameraMovementEnabled(false);
            PlayerManager.SetGravityEnabled(false);
            // if (_enterLoadAreaSFX != null)
            //     DontDestroyOnLoad(_enterLoadAreaSFX.PlayAtPoint(transform.position));
            sceneLoader.LoadScene();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var b = GetComponent<BoxCollider>();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(b.center, new(b.size.x, b.size.y, b.size.z));
        Gizmos.color = new(0, 1, 0, 0.05f);
        Gizmos.DrawCube(b.center, new(b.size.x, b.size.y, b.size.z));
    }

}
