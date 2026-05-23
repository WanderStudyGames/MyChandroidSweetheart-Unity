using UnityEngine;
using UnityEngine.Events;

public class WorldFlagTally : MonoBehaviour
{
    [SerializeField] private string[] worldFlagStrings = new string[] { "" };
    [SerializeField] private IndexedUEvent[] events;

    public void TallyAndExecute()
    {
        int num = 0;
        foreach (string s in worldFlagStrings)
        {
            if (WorldData.WorldFlags.Has(s)) { num++; }
        }
        Execute(num);
    }

    private void Execute(int i)
    {
        foreach (IndexedUEvent e in events)
        {
            if (i <= e.TallyCountMax && i >= e.TallyCountMin) { e.Event.Invoke(); }
        }
    }
    [System.Serializable]
    public class IndexedUEvent
    {
        public int TallyCountMin;
        public int TallyCountMax;
        public UnityEvent Event;
    }
}
