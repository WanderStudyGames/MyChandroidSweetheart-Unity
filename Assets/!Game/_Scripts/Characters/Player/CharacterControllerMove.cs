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

    public Vector3 ApplyFriction(Vector3 velocity, float friction)
    {
        // var scaledFriction = friction * Time.deltaTime * 100;
        // return Vector3.MoveTowards(velocity, Vector3.zero, scaledFriction);
        var damping = 1 - friction * Time.deltaTime * 50;
        if (damping < 0) damping = 0;
        return velocity * damping;
    }
    private float ResetOnVerticalCollision(float velocityY)
    {
        if (IsGrounded) { velocityY = -0.05f; _airControl = true; }
        if (CollisionFlags == CollisionFlags.Above && velocityY > 0) { velocityY = 0f; _airControl = true; }
        return velocityY;
    }
    private bool _airControl = true;
    public bool AirControl => _airControl;
    public void SetVelocity(Vector3 newVelocity, bool airControl = true) { _velocity = newVelocity; _airControl = airControl; }
    public void AddVelocity(Vector3 newVelocity, bool airControl = true)
    {
        _controller.Move(new Vector3(0f, 0.001f, 0f));
        _velocity += newVelocity;
        _airControl = airControl;
    }
    public void Move(Vector3 amount) { _controller.Move(amount); }

    Vector3 ApplyPhysicsLerp(Vector3 currentVel, Vector3 accelDir, float topSpeed, float friction)
    {
        // 1. Where are we trying to go? (The "Room Temp", aka Max Speed)
        // acceleration magnitude / friction = top speed
        Vector3 targetVel = accelDir * topSpeed;

        // 2. The "Pure T" 
        // This calculates exactly how much of the gap to close this frame.
        var percentageOfStartVelocity = Mathf.Exp(-friction * Time.deltaTime);
        float t = 1f - percentageOfStartVelocity;

        // 3. The Lerp
        // We move from our current speed toward the target speed.
        return Vector3.Lerp(currentVel, targetVel, t);
    }
    Vector3 ApplyPhysicsLerp(Vector3 currentVel, Vector3 acceleration, float friction)
    {
        Vector3 targetVel = acceleration / friction;
        var percentageOfStartVelocity = Mathf.Exp(-friction * Time.deltaTime);
        float t = 1f - percentageOfStartVelocity;
        return Vector3.Lerp(currentVel, targetVel, t);
    }

    private Vector3 CalculateDeltaPosition()
    {
        var walkVector = Vector2.zero;

        var h = new Vector2(_velocity.x, _velocity.z);
        //get raw walk vector
        if (_horizontalMovementFuncs.Count > 0)
        {
            walkVector = _horizontalMovementFuncs[0].InvokeFunc(h);
        }

        //start vertical velocity mutation
        if (_gravityEnabled)
            _velocity.y -= _playerMovementProfile.gravity * Time.deltaTime * 0.5f;

        //constant downward force when grounded
        _velocity.y = ResetOnVerticalCollision(_velocity.y);

        Debug.Log("WALK: " + walkVector.magnitude);

        //apply friction
        if (IsGrounded || _airControl)
        {
            h = ApplyPhysicsLerp(h, walkVector, 15f);
            _velocity.x = h.x;
            _velocity.z = h.y;
        }
        else
        {
            // 1. How much speed do we ALREADY have in the direction we want to go?
            float currentSpeedInDir = Vector3.Dot(h, walkVector.normalized);

            // 2. Calculate the 'Headroom' (How much more speed is allowed in this direction?)
            float headroom = (_playerMovementProfile.sprintSpeed / 100f) - currentSpeedInDir;

            // 3. If there is room to add speed, add ONLY what fits
            if (headroom > 0)
            {
                float accelAmount = walkVector.magnitude * Time.deltaTime;

                // We take the SMALLER of the two: 
                // Either the full push OR just the remaining gap.
                float finalPush = Mathf.Min(accelAmount, headroom);

                _velocity.x += walkVector.normalized.x * finalPush;
                _velocity.z += walkVector.normalized.y * finalPush;

            }
        }

        // Debug.Log("VEL: " + _velocity.magnitude);

        //apply movement
        var timeScale = Time.deltaTime * 100 * 0.5f;
        var deltaPositionX = _velocity.x * timeScale;
        var deltaPositionZ = _velocity.z * timeScale;
        var deltaPositionY = _velocity.y * timeScale;

        //finish vertical velocity mutation
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
