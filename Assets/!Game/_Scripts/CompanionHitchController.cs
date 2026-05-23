using System.Linq;
using UnityEngine;

public class CompanionHitchController : MonoBehaviour
{
    [SerializeField] GrapplePoint _grapplePoint;
    [SerializeField] SphereCollider _sphereCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerManager.Tag)) return;

        _grapplePoint.gameObject.SetActive(false);

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PlayerManager.Tag)) return;
        if (PlayerStateManager.State != PlayerStates.Default) return;
        if (_grapplePoint.Attached) return;

        _grapplePoint.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        PlayerManager.OnSetPlayerEnabled += CheckPlayer;
        CheckPlayer();
    }
    private void CheckPlayer(bool b) { CheckPlayer(); }
    private void CheckPlayer()
    {
        if (PlayerStateManager.State == null) return;
        if (PlayerStateManager.State != PlayerStates.Default) return;
        bool playerInRadius = Physics.OverlapSphere(transform.position + _sphereCollider.center, _sphereCollider.radius).Any(col => col.CompareTag(PlayerManager.Tag));
        _grapplePoint.gameObject.SetActive(!playerInRadius);
    }
    private void OnDisable()
    {
        PlayerManager.OnSetPlayerEnabled -= CheckPlayer;
    }
}
