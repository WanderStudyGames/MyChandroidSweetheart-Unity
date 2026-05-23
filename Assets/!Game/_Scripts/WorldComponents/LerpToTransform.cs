using UnityEngine;

public class LerpToTransform : MonoBehaviour
{
    [SerializeField] float lerpSpeed;
    [SerializeField] Transform targetTransform;
    public Transform TargetTransform => targetTransform;
    public bool IncludeRotation = true;
    [SerializeField] private bool _maintainRelativePosition;
    public LerpToTransform SetTarget(Transform t, float speed) { targetTransform = t; lerpSpeed = speed; return this; }
    public void SetTarget(Transform t) { targetTransform = t; }
    private void Awake()
    {
        if (!_maintainRelativePosition) return;
        var go = new GameObject(gameObject.name + "[Anchor]");
        go.transform.parent = targetTransform;
        go.transform.SetPositionAndRotation(transform.position, transform.rotation);
        targetTransform = go.transform;
    }
    void Update()
    {
        if (targetTransform == null) return;
        if (IncludeRotation)
        {
            transform.Lerp(transform, targetTransform, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, lerpSpeed * Time.deltaTime);
        }
    }
}
