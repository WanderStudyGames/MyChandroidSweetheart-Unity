using UnityEngine;
using UnityEngine.Events;

using VInspector;

public class TallyComponent : MonoBehaviour
{
    [Tooltip("The range of how many times this needs to be tallied before it completes. Min and Max are inclusive.")]
    [MinMaxSlider(0, 10), SerializeField] private Vector2Int tallyGoalRange;
    [SerializeField] private UnityEvent OnTally;
    [SerializeField] private UnityEvent OnTallyComplete;
    private int tally = 0;
    public void Execute()
    {
        if (tally >= Random.Range(tallyGoalRange.x, tallyGoalRange.y + 1))
        {
            OnTallyComplete.Invoke();
            tally = 0;
        }
        else
        {
            tally++;
            OnTally.Invoke();
        }
    }
}
