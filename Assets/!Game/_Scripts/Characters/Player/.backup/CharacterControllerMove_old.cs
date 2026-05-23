using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMove : MonoBehaviour
{
    [SerializeField] private PlayerMovementProfile _playerMovementProfile;
    private readonly List<GameFunc<Vector2, Vector2>> _horizontalMovementFuncs = new();
    public List<GameFunc<Vector2, Vector2>> HorizontalMovementFuncs => _horizontalMovementFuncs;

    private readonly List<GameFunc<float, float>> _verticalMovementFuncs = new();
    public List<GameFunc<float, float>> VerticalMovementFuncs => _verticalMovementFuncs;

    private bool _gravityEnabled = true;
    public void SetGravityEnabled(bool enabled) { _gravityEnabled = enabled; }

    private CollisionFlags _collisionFlags;

    private Vector3 _positionOnPreviousFrame;
    public Vector3 PositionOnPreviousFrame => _positionOnPreviousFrame;

    public CollisionFlags CollisionFlags => _collisionFlags;

    private Vector3 _velocity = Vector3.zero;
    public Vector3 Velocity => _velocity;

    private CharacterController _controller;
    public bool IsGrounded => _controller.isGrounded;
    public float SlopeLimit => _controller.slopeLimit;
    public float Radius => _controller.radius;


    private Transform thisTransform;

    private Vector3 _standingSurfaceNormal;
    public string SurfaceBelowTag { get; private set; }

    private bool _isSliding;
    public bool IsSliding() { return CheckIsSliding(); }

    private void Awake()
    {
        thisTransform = transform;
        _controller = GetComponent<CharacterController>();
    }
    public bool GroundBelow()
    {
        if (Physics.SphereCast(transform.position + Vector3.up, _controller.radius * 0.9f, Vector3.down, out RaycastHit hit, 5, _playerMovementProfile.groundLayers, QueryTriggerInteraction.Ignore) && hit.collider.gameObject.tag != "NoLandCheckpoint")
        {
            SurfaceBelowTag = hit.collider.gameObject.tag;
            return true;
        }
        return false;
    }
    private bool CheckIsSliding()
    {
        bool groundBelow = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5, _playerMovementProfile.groundLayers, QueryTriggerInteraction.Ignore);
        //bool groundBelow = Physics.SphereCast(transform.position + Vector3.up, _controller.radius * 0.9f, Vector3.down, out RaycastHit hit, 5, _playerMovementProfile.groundLayers, QueryTriggerInteraction.Ignore);
        //if sphere hit a steep wall, isOnSlope is false.
        bool isOnSlope = Vector3.Angle(hit.normal, Vector3.up) <= 75f;
        if (IsGrounded && groundBelow && isOnSlope)
        {
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null) return false;
            _standingSurfaceNormal = hit.normal;
            return Vector3.Angle(_standingSurfaceNormal, Vector3.up) > _controller.slopeLimit;
        }
        return false;
    }

    public Vector3 ApplyFriction(Vector3 velocity)
    {
        var scaledFriction = _playerMovementProfile.friction * Time.deltaTime * 100;
        velocity = Vector3.MoveTowards(velocity, Vector3.zero, scaledFriction);
        return velocity;
    }
    private float ResetOnVerticalCollision(float velocityY)
    {
        if (IsGrounded) { velocityY = -0.05f; }
        if (CollisionFlags == CollisionFlags.Above && velocityY > 0) { velocityY = 0f; }
        return velocityY;
    }
    public void SetVelocity(Vector3 newVelocity) { _velocity = newVelocity; }
    public void AddVelocity(Vector3 newVelocity)
    {
        _controller.Move(new Vector3(0f, 0.001f, 0f));
        _velocity += newVelocity;
    }
    public void Move(Vector3 amount) { _controller.Move(amount); }

    private Vector3 CalculateDeltaPosition()
    {
        var velocityH = new Vector2(_velocity.x, _velocity.z);
        var deltaVelocityH = Vector2.zero;
        var deltaTime = Time.deltaTime;
        foreach (var gf in _horizontalMovementFuncs)
        {
            deltaVelocityH = gf.InvokeFunc(velocityH);
        }
        if (_gravityEnabled)
            _velocity.y -= _playerMovementProfile.gravity * Time.deltaTime * 0.5f;

        _velocity.y = ResetOnVerticalCollision(_velocity.y);

        if (IsGrounded)
        {
            velocityH = ApplyFriction(velocityH);
        }


        _velocity.x = velocityH.x;
        _velocity.z = velocityH.y;

        var timeScale = Time.deltaTime * 100 * 0.5f;
        var deltaPositionX = (_velocity.x + deltaVelocityH.x * Time.deltaTime);
        var deltaPositionZ = (_velocity.z + deltaVelocityH.y * Time.deltaTime);
        var deltaPositionY = _velocity.y * timeScale;

        if (_gravityEnabled)
            _velocity.y -= _playerMovementProfile.gravity * Time.deltaTime * 0.5f;

        return new Vector3(deltaPositionX, deltaPositionY, deltaPositionZ) * Mathf.Clamp01(Time.timeScale);

    }
    private void Update()
    {
        _positionOnPreviousFrame = thisTransform.position;
        _collisionFlags = _controller.Move(CalculateDeltaPosition());
        PlayerData.RecordPositionRotation(transform.position, transform.rotation);
        _isSliding = CheckIsSliding();
        if (_isSliding)
        {
            _controller.Move(5f * Time.deltaTime * new Vector3(_standingSurfaceNormal.x, -_standingSurfaceNormal.y, _standingSurfaceNormal.z));
            _collisionFlags = _controller.Move(Vector3.down * 200f);
        }
    }

}
