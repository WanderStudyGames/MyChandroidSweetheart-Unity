using System.Collections.Generic;
using UnityEngine;

public class SmallTalkController : MonoBehaviour
{
    [SerializeField] private List<SmallTalk> _smallTalks = new();
    private int _index;

    private void Awake()
    {
        foreach (SmallTalk s in _smallTalks) { s.gameObject.SetActive(false); }
    }
    public void Speak()
    {
        foreach (SmallTalk smallTalk in _smallTalks) smallTalk.gameObject.SetActive(false);
        _smallTalks[_index].gameObject.SetActive(true);
        _index++;
        if (_index >= _smallTalks.Count) { _index = 0; }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            foreach (SmallTalk smallTalk in _smallTalks) smallTalk.DisableQuick();
        }
    }
}

