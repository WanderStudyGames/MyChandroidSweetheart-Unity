using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ForceFieldBridge : DeviceComponent
{
    [SerializeField] private AttachablePlatform _attachablePlatform;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;
    [SerializeField] private ShaderKeywordLerp _shaderKeywordLerp;
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private UnityEvent _Off;
    [SerializeField] private UnityEvent _On;
    [SerializeField] private Transform[] _safePoints;
    public override void Int(int i)
    {
    }
    private void Awake()
    {
        _navMeshObstacle.enabled = true;
        _shaderKeywordLerp.LerpToOne(0.1f);
        _collider.enabled = false;
        _meshRenderer.enabled = false;
    }

    public override void Off()
    {
        _Off.Invoke();
        _navMeshObstacle.enabled = true;
        _collider.enabled = false;
        _attachablePlatform.TeleportNPCsToClosest(_safePoints);
        _shaderKeywordLerp.LerpToOne(0.3f);
        StartCoroutine(Co_Renderer());
        IEnumerator Co_Renderer()
        {
            yield return new WaitForSeconds(0.3f);
            _meshRenderer.enabled = false;
        }
    }

    public override void On()
    {
        _On.Invoke();
        _navMeshObstacle.enabled = false;
        _collider.enabled = true;
        _meshRenderer.enabled = true;
        _shaderKeywordLerp.LerpToZero(0.3f);
    }

    public override void SingleClick()
    {
        On();
    }
}
