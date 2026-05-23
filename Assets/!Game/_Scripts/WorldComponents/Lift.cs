using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VInspector;
public class Lift : DeviceComponent
{
    [SerializeField] private List<LiftDestination> _destinations;
    public enum MoveMode { Speed, Duration };
    [HideInInspector][SerializeField] private Vector3[] positions = { Vector3.zero, Vector3.up };
    public Vector3[] Positions => positions;
    private Vector3 startingPosition = Vector3.zero;
    private bool paused;
    public void Pause() { paused = true; }
    public void Unpause() { paused = false; }

    [SerializeField] private MoveMode _moveMode = MoveMode.Speed;

    [Header("Speed"), ShowIf("_moveMode", MoveMode.Speed)]
    [Range(0.1f, 100f)][SerializeField] private float maxSpeed = 5f;
    public void SetMaxSpeed(float speed)
    {
        if (_moveMode == MoveMode.Speed)
            maxSpeed = speed;
        //derive duration from speed and distance
        else if (_moveMode == MoveMode.Duration && _destinations.Count > 1)
        {
            float distance = Vector3.Distance(_destinations[0].transform.position, _destinations[1].transform.position);
            _durationInSeconds = distance / speed;
        }
    }

    [Header("Duration")]
    [SerializeField, ShowIf("_moveMode", MoveMode.Duration)] private float _durationInSeconds = 2;
    public void SetDuration(float duration)
    {
        if (_moveMode == MoveMode.Duration)
            _durationInSeconds = duration;
        else if (_moveMode == MoveMode.Speed && _destinations.Count > 1)
        {
            float distance = Vector3.Distance(_destinations[0].transform.position, _destinations[1].transform.position);
            maxSpeed = distance / duration;
        }
    }
    [Separator, EndIf]
    [SerializeField] private AnimationCurve _animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Separator]
    [SerializeField] private UnityEvent<int> startAction;
    [SerializeField] private UnityEvent<int> finishAction;
    [SerializeField] public event Action<int> OnStart;
    [SerializeField] public event Action<int> OnFinish;
    [SerializeField] SFX startSFX;
    [SerializeField] SFX loopSFX;
    [SerializeField] SFX endSFX;
#if UNITY_EDITOR
    [SerializeField] bool alwaysDrawGizmos;
#endif
    [SerializeField] bool pingPong;
    private int iterator = 1;


    private int _destinationIndex = 0;
    private bool _moving;

    [SerializeField] private AttachablePlatform _attachablePlatform;
    [SerializeField] private Lift _parent;
    private List<Lift> children = new();
    public void AddChild(Lift child)
    {
        children.AddUnique(child);
    }
    public void RemoveChild(Lift child)
    {
        children.RemoveAll(child);
    }

    AudioSource audioSource;
    AudioSource loopAudioSource;

    private void Awake()
    {
        startingPosition = transform.position;
        audioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        if (_parent == null || _parent == this) return;
        _parent.AddChild(this);
    }
    private void OnDestroy()
    {
        if (_parent == null || _parent == this) return;
        _parent.RemoveChild(this);
    }

    public bool Contains(IAttachableCharacter character)
    {
        //contains needs to be a recursive check through children
        foreach (var child in children)
        {
            if (child.Contains(character)) return true;
        }
        return _attachablePlatform != null && _attachablePlatform.Contains(character);
    }

    public void End()
    {
        StopAllCoroutines();

        _moving = false;
        finishAction.Invoke(_destinationIndex);
        OnFinish?.Invoke(_destinationIndex);
        loopAudioSource.Stop();
        if (endSFX != null) audioSource.PlaySFX(endSFX);
        if (Moving(this)) return;
        StartCoroutine(Co_Detach());
        IEnumerator Co_Detach()
        {
            yield return null;
            Debug.Log("Detaching NPCs from lift");
            DetachNPCs();
        }
    }
    public bool Moving(Lift terminator)
    {
        bool childMoving = false;
        foreach (var child in children)
        {
            if (child == terminator) continue;
            if (child.Moving(terminator)) childMoving = true;
        }
        return _moving || (_parent != null && _parent != terminator && _parent.Moving(terminator)) || childMoving;
    }

    public void MoveNext()
    {
        _destinationIndex += iterator;
        if (_destinationIndex >= _destinations.Count || _destinationIndex <= -1)
        {
            if (!pingPong) { _destinationIndex = 0; }
            else
            {
                iterator = -iterator;
                _destinationIndex += iterator * 2;
            }
        }
        Move(_destinations[_destinationIndex].transform);
    }

    public void Move(int i)
    {
        if (_destinationIndex == i) return;
        if (i < _destinations.Count && i > -1)
        {
            _destinationIndex = i;
            Move(_destinations[i].transform);
        }
    }
    public void AttachNPCs()
    {
        _attachablePlatform?.AttachNPCS();
        foreach (var child in children)
        {
            child.AttachNPCs();
        }
    }
    public void DetachNPCs()
    {
        _attachablePlatform?.DetachNPCS();
        foreach (var child in children)
        {
            child.DetachNPCs();
        }
    }
    public void Move(Transform destination)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Move());
        IEnumerator Co_Move()
        {
            _moving = true;

            AttachNPCs();

            startAction.Invoke(_destinationIndex);
            OnStart?.Invoke(_destinationIndex);

            if (startSFX != null) audioSource.PlaySFX(startSFX);

            if (loopSFX != null) loopAudioSource.PlaySFX(loopSFX);

            Vector3 start = transform.localPosition;
            var end = transform.parent.InverseTransformPoint(destination.position);

            float time = 0;
            var duration = _durationInSeconds;
            if (_moveMode == MoveMode.Speed)
            {
                duration = Vector3.Distance(start, end) / maxSpeed;
            }

            while (time < duration)
            {
                if (paused)
                {
                    yield return null;
                    continue;
                }
                //convert dest position to local space

                transform.localPosition = Vector3.Lerp(start, end, _animCurve.Evaluate(time / duration));
                //Physics.SyncTransforms();
                time += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = end;
            //Physics.SyncTransforms();
            End();
        }
    }


#if UNITY_EDITOR
    private void DrawGizmos()
    {

        if (Application.isPlaying || _destinations == null) return;
        for (int i = 0; i < _destinations.Count; i++)
        {
            if (_destinations[i] == null) continue;
            var mf = _destinations[i].MeshFilter;
            Gizmos.color = new(0f, 0f, 1f, 0.3f);
            Gizmos.color = Color.green;
            //draw spheres at each destination
            Gizmos.DrawSphere(_destinations[i].transform.position, 0.2f);
            if (i > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_destinations[i].transform.position, _destinations[i - 1].transform.position);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (alwaysDrawGizmos) return;
        DrawGizmos();
    }
    private void OnDrawGizmos()
    {
        if (!alwaysDrawGizmos) return;
        DrawGizmos();
    }
#endif
    public override void On()
    {
        Move(1);
    }

    public override void Off()
    {
        Move(0);
    }

    public override void SingleClick()
    {
        MoveNext();
    }

    public override void Int(int i)
    {
        Move(i);
    }

}
