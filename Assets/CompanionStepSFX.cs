using UnityEngine;

public class CompanionStepSFX : MonoBehaviour
{
    [SerializeField] private StepSoundBank _stepSoundBank;
    [SerializeField] private SFX _carryingSFX;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private LayerMask _floorLayerMask;
    [SerializeField] private Companion _companion;
    [SerializeField] private MeshVelocityTracker _velocityTracker;
    [SerializeField] private ParticleSystem _dustParticleSystem;
    private AnimationCurve _velocityToVolumeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public void Step()
    {
        // Debug.Log(_velocityTracker.GetVelocity());
        if (_velocityTracker.GetVelocity().magnitude < 0.65f)
        {
            return;
        }
        _dustParticleSystem.Play();
        if (_companion.CarriedObject != null)
        {
            _audioSource.PlaySFX(_carryingSFX);
            return;
        }
        //raycast to floor
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 2, _floorLayerMask, QueryTriggerInteraction.Ignore))
        {
            var sfx = _stepSoundBank.GetStepSoundForTag(hit.collider.tag).StepSFX;
            // Play the sound effect
            _audioSource.SetFromSFX(sfx);
            _audioSource.pitch *= 2f;
            _audioSource.spatialBlend = 1;
            var volumeT = Mathf.InverseLerp(0.65f, 1f, _velocityTracker.GetVelocity().magnitude);
            _audioSource.volume *= 5f * _velocityToVolumeCurve.Evaluate(volumeT);
            _audioSource.spread = 0f;
            _audioSource.Play();
        }
    }
}
