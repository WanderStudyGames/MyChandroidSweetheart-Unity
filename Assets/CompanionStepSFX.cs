using UnityEngine;

public class CompanionStepSFX : MonoBehaviour
{
    [SerializeField] private StepSoundBank _stepSoundBank;
    [SerializeField] private SFX _carryingSFX;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private LayerMask _floorLayerMask;
    [SerializeField] private Companion _companion;
    public void Step()
    {
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
            _audioSource.volume *= 5f;
            _audioSource.spread = 0f;
            _audioSource.Play();
        }
    }
}
