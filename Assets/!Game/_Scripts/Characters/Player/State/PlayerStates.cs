using UnityEngine;
//finish implementing
[CreateAssetMenu(menuName = "ScriptableObjects/Player/States/PlayerStates", fileName = "PlayerStates")]
public class PlayerStates : ScriptableObject
{
    private static PlayerStates instance;
    private void OnEnable()
    {
        instance = this;
    }

    [SerializeField] private DefaultPlayerState _defaultPlayerState;
    public static DefaultPlayerState Default => instance._defaultPlayerState;

    [SerializeField] private ScannerPlayerState _scanner;
    public static ScannerPlayerState Scanner => instance._scanner;

    [SerializeField] private DialoguePlayerState _dialogue;
    public static DialoguePlayerState Dialogue => instance._dialogue;

    [SerializeField] private ScanningPlayerState _scanning;
    public static ScanningPlayerState Scanning => instance._scanning;

    [SerializeField] private DialogueFrozenPlayerState _dialogueFrozen;
    public static DialogueFrozenPlayerState DialogueFrozen => instance._dialogueFrozen;
    [SerializeField] private MenuPlayerState _menu;
    public static MenuPlayerState Menu => instance._menu;
    [SerializeField] private CarryingPlayerState _carrying;
    public static CarryingPlayerState Carrying => instance._carrying;
    [SerializeField] private RewiringPlayerState _rewiring;
    public static RewiringPlayerState Rewiring => instance._rewiring;
    [SerializeField] private TabletPlayerState _tablet;
    public static TabletPlayerState Tablet => instance._tablet;
}

