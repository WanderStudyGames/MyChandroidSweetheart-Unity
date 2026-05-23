using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStateManager))]
public class PlayerLook : MonoBehaviour
{
    private static PlayerLook instance;
    [Dependency]
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    public static Camera Camera => instance._camera;
    /// <summary>
    /// Sets the parent transform used for horizontal rotation. By default, this is the player transform, but it can be set to something else (like a tablet anchor) if needed. Vertical rotation is always applied to the camera transform.
    /// </summary>
    /// <param name="t"></param>
    public static void SetRotationTransform(Transform t)
    {
        instance.RotationTransform = t;
    }
    /// <summary>
    /// The parent transform used for horizontal rotation. By default, this is the player transform, but it can be set to something else (like a tablet anchor) if needed. Vertical rotation is always applied to the camera transform.
    /// </summary>
    public Transform RotationTransform { get; set; }
    /// <summary>
    /// Returns the transform that should be used for horizontal rotation. By default, this is the player transform, but it can be set to something else (like a tablet anchor) if needed. Vertical rotation is always applied to the camera transform.
    /// </summary>
    public static Transform GetRotationTransform() => instance.RotationTransform;

    [SerializeField] private PlayerLookProfile playerLookProfile;

    private Transform lerpObject;

    private bool isMouse = true;

    private bool _lerping;

    private float MouseSpeed => isMouse ? playerLookProfile.LookSpeed : playerLookProfile.LookSpeed * Time.deltaTime * 100;
    private bool InvertLook => playerLookProfile.InvertLook;
    private Vector2 HorizontalClamps => playerLookProfile.ClampX;
    private Vector2 VerticalClamps => playerLookProfile.ClampY;
    private void OnControlsChanged(PlayerInput playerInput)
    {
        isMouse = playerInput.currentControlScheme == "Keyboard & Mouse";
    }
    private void Start()
    {
        isMouse = _playerInput.currentControlScheme == "Keyboard & Mouse";
    }
    private void OnEnable()
    {
        if (instance != null && instance != this) instance.enabled = false;
        instance = this;

        if (_lerping)
        {
            LerpTowardsObject(null, 200f);
        }

        if (playerLookProfile.ClampXEnabled)
        {
            StartCoroutine(FixHorizontalRotation());
        }

        mouseDelta = Vector2.zero;

        _playerInput.onControlsChanged += OnControlsChanged;
        _playerInput.actions["Look"].performed += OnMouseMove;
        _playerInput.actions["Look"].canceled += OnMouseMove;
    }
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= OnControlsChanged;
        _playerInput.actions["Look"].performed -= OnMouseMove;
        _playerInput.actions["Look"].canceled -= OnMouseMove;

    }

    public void SetComponentProfile(ComponentProfile profile)
    {
        playerLookProfile = (PlayerLookProfile)profile;
    }

    private Vector2 mouseDelta = Vector2.zero;
    private float cameraXRotation;

    public Camera GetCamera() { return _camera; }
    private void Awake()
    {
        RotationTransform = transform;
        Cursor.lockState = CursorLockMode.Locked;
        //instance = this;
    }
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (_lerping)
        {
            mouseDelta = Vector2.zero;
            return;
        }
        if (context.canceled)
        {
            mouseDelta = Vector2.zero;
            return;
        }
        if (context.action.WasPerformedThisFrame())
        {
            mouseDelta = (Vector2)context.ReadValueAsObject();
        }
    }

    public void LerpTowardsObject(Transform tr, float lerpSpeed)
    {
        _lerping = true;
        lerpObject = tr;
        StopAllCoroutines();
        if (lerpObject == null)
        {
            StopAllCoroutines();
            cameraXRotation = _camera.transform.rotation.eulerAngles.x;
            Debug.LogWarning(cameraXRotation);
            if (cameraXRotation < VerticalClamps.x) cameraXRotation += 360f;
            if (cameraXRotation > VerticalClamps.y) cameraXRotation -= 360f;
            _lerping = false;
            //StartCoroutine(Co_CameraYReset(lerpSpeed));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Co_LerpTowardsObject(lerpSpeed));
        }
    }

    IEnumerator Co_CameraYReset(float lerpSpeed)
    {
        var localRotTarget = _camera.transform.localEulerAngles;
        localRotTarget.x = 0;
        bool b = true;
        yield return null;
        while (b)
        {
            _camera.transform.localRotation = Quaternion.RotateTowards(_camera.transform.localRotation, Quaternion.Euler(localRotTarget), lerpSpeed * Time.deltaTime);
            if (_camera.transform.localRotation.eulerAngles.x == 0f) b = false;
            yield return null;
        }
        cameraXRotation = 0;
        _lerping = false;
    }

    private IEnumerator Co_LerpTowardsObject(float lerpSpeed)
    {
        //if (lerpObject == null) yield break;
        while (lerpObject != null)
        {
            float yGoal = Quaternion.LookRotation(lerpObject.transform.position - transform.position, Vector3.up).eulerAngles.y;
            Vector3 localBodyRotTarget = RotationTransform.localRotation.eulerAngles;
            localBodyRotTarget.y = yGoal;
            float xGoal = Quaternion.LookRotation(lerpObject.transform.position - _camera.transform.position, _camera.transform.right).eulerAngles.x;

            Vector3 localCamRotTarget = _camera.transform.localRotation.eulerAngles;

            localCamRotTarget.x = xGoal;

            RotationTransform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(localBodyRotTarget), lerpSpeed * Time.deltaTime);
            _camera.transform.localRotation = Quaternion.Lerp(_camera.transform.localRotation, Quaternion.Euler(localCamRotTarget), lerpSpeed * Time.deltaTime);

            yield return null;
        }



    }
    public static void SetCameraEnabled(bool b)
    {
        instance._camera.enabled = b;
        UIManager.SetUIEnabled(b);
    }

    private IEnumerator FixHorizontalRotation()
    {
        if (HorizontalClamps.x > HorizontalClamps.y)
            while (true)
            {
                var rotation = transform.localEulerAngles.y;
                if (rotation < HorizontalClamps.x && rotation > 180f) { transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(new Vector3(0, HorizontalClamps.x, 0))); }
                if (rotation > HorizontalClamps.y && rotation < 180f) { transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(new Vector3(0, HorizontalClamps.y, 0))); }
                yield return null;
            }
        else
            while (true)
            {
                var rotation = transform.localEulerAngles.y;
                if (rotation < HorizontalClamps.x) { transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(new Vector3(0, HorizontalClamps.x, 0))); }
                if (rotation > HorizontalClamps.y) { transform.SetLocalPositionAndRotation(transform.localPosition, Quaternion.Euler(new Vector3(0, HorizontalClamps.y, 0))); }
                yield return null;
            }
    }

    private void Update()
    {
        RotationTransform.Rotate(Vector3.up, mouseDelta.x * MouseSpeed);

        float mouseDeltaY = (InvertLook ? mouseDelta.y : -mouseDelta.y) * MouseSpeed;

        cameraXRotation += mouseDeltaY;

        if (cameraXRotation < VerticalClamps.x)
        {
            _camera.transform.SetLocalPositionAndRotation(_camera.transform.localPosition, Quaternion.Euler(new Vector3(VerticalClamps.x, 0, 0)));
            cameraXRotation = VerticalClamps.x;
        }
        else if (cameraXRotation > VerticalClamps.y)
        {
            _camera.transform.SetLocalPositionAndRotation(_camera.transform.localPosition, Quaternion.Euler(new Vector3(VerticalClamps.y, 0, 0)));
            cameraXRotation = VerticalClamps.y;
        }
        else
        {
            _camera.transform.Rotate(Vector3.right, mouseDeltaY);
        }


    }
}
