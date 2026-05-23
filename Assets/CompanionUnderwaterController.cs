using UnityEngine;
using UnityEngine.Events;

public class CompanionUnderwaterController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private UnityEvent onEnterWater;
    [SerializeField] private UnityEvent onExitWater;
    [SerializeField] private ParticleSystem underwaterParticles;
    [SerializeField] private InteractibleObject _interactibleObject;
    private bool isUnderwater = false;
    void Start()
    {

    }

    private bool IsUnderwater()
    {
        return Physics.Raycast(transform.position - (Vector3.up * 0.85f), Vector3.up, out _, 100f, waterLayer);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isUnderwater && IsUnderwater())
        {
            isUnderwater = true;
            _interactibleObject.AddEmbargo("Underwater");
            onEnterWater.Invoke();
            Debug.Log("Entered water");
            if (PlayerStateManager.State != PlayerStates.Tablet)
            {
                underwaterParticles.Play();
            }
            // Implement underwater behavior here
        }
        else if (isUnderwater && !IsUnderwater())
        {
            isUnderwater = false;
            _interactibleObject.RemoveEmbargo("Underwater");
            onExitWater.Invoke();
            Debug.Log("Exited water");
            underwaterParticles.Stop();
            // Implement above water behavior here
        }
    }
}
