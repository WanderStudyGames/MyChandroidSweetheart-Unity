using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SciFiNerd : MonoBehaviour
{
    [SerializeField] private LerpOnCommand _lerpOnCommand;
    [SerializeField] private GameObject _lookCollider;
    [SerializeField] private CompanionInteractible _companionInteractible;
    [SerializeField] private int _health = 3;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private UnityEvent _onSeen;
    [SerializeField] private UnityEvent _onSneak;
    [SerializeField] private UnityEvent _onTeleport;
    private bool _seen = false;
    private void Awake()
    {
        _companionInteractible.enabled = false;
        TabletPlayerState.Jammed = true;
        Teleport();
    }
    private void OnEnable()
    {

        PlayerStates.Tablet.OnStateEnableEvent += OnTabletEnable;
        PlayerStates.Tablet.OnStateDisableEvent += OnTabletDisable;
    }
    private void OnDisable()
    {
        PlayerStates.Tablet.OnStateEnableEvent -= OnTabletEnable;
        PlayerStates.Tablet.OnStateDisableEvent -= OnTabletDisable;
    }
    private void OnTabletEnable()
    {
        _lookCollider.SetActive(false);
    }
    private void OnTabletDisable()
    {
        _lookCollider.SetActive(true);
    }
    public void TeleportRandom()
    {
        var randomInt = 0;
        do
        {
            randomInt = Random.Range(0, _lerpOnCommand.TargetCount - 1);

        } while (randomInt == _lerpOnCommand.CurrentIndex);
        _lerpOnCommand.TeleportTo(randomInt);

        _onTeleport.Invoke();
    }
    public void Teleport()
    {
        _seen = false;
        TabletPlayerState.Jammed = true;
        _companionInteractible.enabled = false;
        TeleportRandom();
        _lookCollider.SetActive(false);
        StartCoroutine(Co_Teleport());
        IEnumerator Co_Teleport()
        {
            yield return null;
            _lookCollider.SetActive(true);
            var count = 0;
            var iterations = Random.Range(2, 8);
            while (count < iterations)
            {
                count++;
                yield return new WaitForSeconds(0.7f);
                TeleportRandom();
            }
            yield return new WaitForSeconds(1f);
            Sneak();
        }
    }
    public void TakeDamage()
    {
        _health--;
        if (_health <= 0)
        {
            _onDie.Invoke();
        }
        _companionInteractible.enabled = false;
        CompanionManager.StandInPlace();
        Teleport();
    }
    public void SeenByPlayer()
    {
        if (_seen) return;
        _companionInteractible.enabled = false;
        _onSeen.Invoke();
        _seen = true;
        StopAllCoroutines();
        StartCoroutine(Co_Seen());
        IEnumerator Co_Seen()
        {
            _onSeen.Invoke();
            yield return new WaitForSeconds(1f);
            Teleport();
            _seen = false;
        }
    }
    public void Sneak()
    {
        StopAllCoroutines();
        StartCoroutine(Co_Sneak());
        IEnumerator Co_Sneak()
        {
            _onSneak.Invoke();
            _companionInteractible.enabled = true;
            TabletPlayerState.Jammed = false;
            _seen = false;
            //move slowly towards player
            while (Vector3.Distance(transform.position, PlayerData.Position) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerData.Position, Time.deltaTime * 0.5f);
                yield return null;
            }
            TabletPlayerState.Jammed = true;
            _companionInteractible.enabled = false;
            PlayerManager.Die(null, null);
        }
    }
}
