using UnityEngine;

[CreateAssetMenu(fileName = "ScanUIProfile", menuName = "ScriptableObjects/UI/ScanUIProfile")]
[ProfileFor(typeof(ScanUI))]
public class ScanUIProfile : UIComponentProfile
{

    public GameEvent OnDisplayTextGE;

    [Range(0, 0.1f)][SerializeField] private float _scrollDelay;
    [Range(0, 0.5f)][SerializeField] private float _scrollDelayName;
    public float ScrollDelay => _scrollDelay;
    public float ScrollDelayName => _scrollDelayName;

    [Range(0, 300)][SerializeField] private int _charLimit;
    public int CharLimit => _charLimit;

    public SFX scanAmbienceSFX;
    //public SFX scanEnterSFX;
    public SFX scanStartSFX;
    public SFX scanStopSFX;
    //public SFX scanExitSFX;
    public SFX scanPageSkipSFX;

}