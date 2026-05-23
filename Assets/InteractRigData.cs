using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InteractRigData : MonoBehaviour
{
    [SerializeField] private Rig[] _rigs;
    public Rig[] Rigs => _rigs;
    [SerializeField] private Vector3 _timings;
    public Vector3 Timings => _timings;
    [SerializeField] private bool _lookAtPlayer;
    public bool LookAtPlayer => _lookAtPlayer;
}
