using System.Collections;
using UnityEngine;
public class Door : DeviceComponent
{
    [SerializeField] private Transform _closedPosition;
    [SerializeField] private Transform _openPosition;
    [SerializeField] private float _transitionDurationInSeconds;
    [SerializeField] private SFX _openSFX;
    [SerializeField] private SFX _closeSFX;
    private bool _open = false;

    public override void Int(int i)
    {
        i = (int)Mathf.Clamp01(i);
        if (i == 1) { On(); }
        else { Off(); }
    }

    public override void Off()
    {
        if (!_open) return;
        _closeSFX?.PlayAtPoint(transform.position);
        Move(_closedPosition);
        _open = false;
    }

    public override void On()
    {
        if (_open) return;
        _openSFX?.PlayAtPoint(transform.position);
        Move(_openPosition);
        _open = true;
    }

    public override void SingleClick()
    {
        if (!_open) { On(); }
        else { Off(); }
    }
    public void Move(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Move());
        IEnumerator Co_Move()
        {
            yield return StartCoroutine(transform.Co_LerpOverTime(target, _transitionDurationInSeconds));//
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (_openPosition == null) return;
        if (_closedPosition == null) return;
        var tr = _openPosition.transform;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, tr.position, tr.rotation, tr.lossyScale);
        tr = _closedPosition.transform;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, tr.position, tr.rotation, tr.lossyScale);
    }
}
