using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerDive : MonoBehaviour
{
    [SerializeField, Dependency] private CharacterControllerMove _characterControllerMove;
    [SerializeField, Dependency] private InputActionReference _jumpActionReference;
    [SerializeField] private LayerMask _floorLayerMask;
    [SerializeField] private float _bouyancyAcceleration = 3f;
    [SerializeField] private UnityEvent _onSurface;
    private Vector3 _initialLocalPosition;
    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        PlayerJump.OnLandWater += Dive;
        PlayerStates.Tablet.OnStateEnableEvent += OnTabletEnable;
    }
    private void OnTabletEnable()
    {
        transform.localPosition = _initialLocalPosition;
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        PlayerJump.OnLandWater -= Dive;
        PlayerStates.Tablet.OnStateEnableEvent -= OnTabletEnable;
    }

    public void Dive(float velocity)
    {
        _jumpActionReference.action.Disable();
        velocity *= 50f;
        StopAllCoroutines();
        transform.localPosition = new(
            transform.localPosition.x,
            transform.localPosition.y + velocity * Time.deltaTime,
            transform.localPosition.z
            );
        StartCoroutine(Co_Dive());
        Debug.Log(velocity);
        IEnumerator Co_Dive()
        {
            Debug.Log(transform.localPosition.y + ", " + _initialLocalPosition.y);
            while (transform.localPosition.y < _initialLocalPosition.y)
            {
                velocity += _bouyancyAcceleration * Time.deltaTime * 0.5f;
                transform.localPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y + velocity * Time.deltaTime,
                    transform.localPosition.z
                    );
                //reset velocity to 0 if we hit the floor
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f, _floorLayerMask, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log("Hit floor during dive");
                    transform.localPosition += 0.5f * Vector3.up;
                    if (velocity < 0f) velocity = 2f;
                }

                velocity += _bouyancyAcceleration * Time.deltaTime * 0.5f;
                yield return null;
            }
            _characterControllerMove.AddVelocity(new(0f, velocity / 50f, 0f));
            transform.localPosition = _initialLocalPosition;
            _jumpActionReference.action.Enable();
            _onSurface.Invoke();
        }


    }
}
