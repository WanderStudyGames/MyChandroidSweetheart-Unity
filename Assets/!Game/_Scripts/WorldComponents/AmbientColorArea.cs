using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientColorArea : MonoBehaviour
{
    [SerializeField] [Dependency] private PlayerManager playerManager;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float lerpSpeed = 0.3f;
    [SerializeField] private Vector3 colliderSize = Vector3.one;
    private Color sceneColor;
    private Color currentSceneColor;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Awake()
    {
        currentSceneColor = RenderSettings.ambientLight;
        sceneColor = currentSceneColor;
        boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = Vector3.zero;
        boxCollider.size = colliderSize;
    }
    private void OnDisable()
    {
        RenderSettings.ambientLight = sceneColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            StopAllCoroutines();
            StartCoroutine(LerpToColor(color));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            StopAllCoroutines();
            StartCoroutine(LerpToColor(sceneColor));
        }
    }

    IEnumerator LerpToColor(Color finalColor)
    {
        while(currentSceneColor != finalColor)
        {
            currentSceneColor = Color.Lerp(currentSceneColor, finalColor, lerpSpeed * Time.deltaTime);
            RenderSettings.ambientLight = currentSceneColor;
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Vector3.zero + transform.position, Vector3.Scale(colliderSize, transform.lossyScale));
    }
}
