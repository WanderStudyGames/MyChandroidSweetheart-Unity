using UnityEngine;

[RequireComponent(typeof(PlayerSwim))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] PlayerMovementProfile _playerMovementProfile;
    [SerializeField] SFX jumpSFX => _playerSwim.IsSwimming ? _playerMovementProfile.splashSFX : _playerMovementProfile.concreteJumpSFX;
    [SerializeField] SFX fallingSFX;
    [SerializeField] private ParticleSystem _stepParticleSystem;
    [SerializeField] private ParticleSystem _landParticleSystem;
    [SerializeField] private InstantiatePrefab _splashParticleSystem;
    private SFX LandSFX(string tag, float velocity)
    {
        if (!_playerSwim.IsSwimming)
        {
            var sfx = stepsSoundBank.GetLandSFX(velocity, tag);
            //if (tag == "Grass" || tag == "Leaves") return sfx;

            if (Mathf.Abs(velocity) > stepsSoundBank.StepThreshholds.x)
            {
                if (tag == "Shallow")
                    _splashParticleSystem.Instantiate();
                else
                    _landParticleSystem.Play();
            }
            else { _stepParticleSystem.Play(); }

            return sfx;
        }
        return _playerMovementProfile.splashSFX;
    }

    private SFX StepSFX(string tag)
    {
        if (!_playerSwim.IsSwimming)
        {
            _stepParticleSystem.Play();
            return stepsSoundBank.GetStepSoundForTag(tag).StepSFX;
        }
        return _playerMovementProfile.swimSFX;
    }

    private SFX JumpSFX(string tag)
    {
        if (!_playerSwim.IsSwimming)
        {
            if (tag == "Shallow")
                _splashParticleSystem.Instantiate();
            else
                _landParticleSystem.Play();
            return stepsSoundBank.GetStepSoundForTag(tag).JumpSFX;
        }
        return _playerMovementProfile.swimSFX;
    }

    private PlayerSwim _playerSwim;

    private AudioSource _step;
    private AudioSource _jump;
    private AudioSource _land;
    private AudioSource _falling;
    [SerializeField] private StepSoundBank stepsSoundBank;
    public bool LandIsPlaying => _land.isPlaying;

    private void Awake()
    {
        _step = gameObject.AddComponent<AudioSource>();
        _step.playOnAwake = false;
        _jump = gameObject.AddComponent<AudioSource>();
        _jump.playOnAwake = false;
        _land = gameObject.AddComponent<AudioSource>();
        _land.playOnAwake = false;
        _falling = gameObject.AddComponent<AudioSource>();
        _falling.playOnAwake = false;

        _playerSwim = GetComponent<PlayerSwim>();
    }
    private void OnEnable()
    {
        PlayerMove.OnPlayerStep += Step;
    }
    private void OnDisable()
    {
        PlayerMove.OnPlayerStep -= Step;
    }
    public void Step(string tag)
    {
        if (tag == "Shallow" && _step.isPlaying) return;
        _step.PlaySFX(StepSFX(tag));
    }
    public void Jump(string tag, bool checkIsPlaying = false)
    {
        if (checkIsPlaying && _jump.isPlaying) return;
        _jump.PlaySFX(JumpSFX(tag));
    }
    public void Land(string tag, float yVelocity, bool checkIsPlaying = false)
    {
        if (yVelocity == 0) return;
        if (_land.isPlaying) return;


        _land.PlaySFX(LandSFX(tag, yVelocity));
    }
    public AudioSource StartFalling()
    {
        _falling.PlaySFX(fallingSFX, 0);
        return _falling;
    }
    public void StopFalling()
    {
        _falling.Stop();
    }

}
