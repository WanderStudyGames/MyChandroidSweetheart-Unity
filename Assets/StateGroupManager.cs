using System;

using UnityEngine;

public class StateGroupManager : MonoBehaviour
{
    private int _members;
    public event Action<int> OnGroupEnter;
    public event Action<int> OnGroupExit;
    public void AddActiveGroup(int ID)
    {
        _members += 1;
        if (_members == 1)
        {
            OnGroupEnter?.Invoke(ID);
        }
    }
    public void RemoveActiveGroup(int ID)
    {
        _members -= 1;
        if (_members == 0)
        {
            OnGroupExit?.Invoke(ID);
        }
    }
    public bool GroupIsActive(int ID)
    {
        return _members > 0;
    }
}
